# FizzBuzz API

Web API (.NET 10 / ASP.NET Core) that generates configurable FizzBuzz sequences and
exposes usage statistics. Interactive documentation is available through Swagger.


## Prerequisites

| To... | You need... |
|-------|-------------|
| Run with Docker | [Docker](https://docs.docker.com/get-docker/) (with Docker Compose, included in Docker Desktop) |
| Run locally | The [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) |

## Quick start with Docker (recommended)

No .NET installation required: Docker handles everything.

```bash
# From the project root
docker compose up --build
```

The API is then available at **http://localhost:8080**.

Open the Swagger UI to explore and test the endpoints interactively: **http://localhost:8080/swagger**

To stop it:

```bash
# Ctrl+C if running in the foreground, otherwise:
docker compose down
```

## Running locally (without Docker)

```bash
dotnet run --project FizzBuzz/FizzBuzz.csproj
```

When run locally, the application starts in the `Development` environment (see
`FizzBuzz/Properties/launchSettings.json`) at **http://localhost:5135**
(and https://localhost:7134).

Run the tests:

```bash
dotnet test
```
## Docker details

The [`Dockerfile`](Dockerfile) uses a **multi-stage build**:

1. **`build` stage** : .NET 10 SDK image (heavy): restores the NuGet dependencies, then
   publishes the application in `Release` configuration. The restore is done in a separate
   layer (the `.csproj` is copied before the rest of the code) to take advantage of
   Docker's layer cache: packages are re-downloaded only when the `.csproj` changes.
2. **`final` stage** : ASP.NET runtime image (lightweight): copies only the published
   binaries. The shipped image contains neither the SDK nor the source code → smaller and
   more secure.

Network configuration:

- Kestrel listens on port **8080** inside the container (`ASPNETCORE_HTTP_PORTS`).
- The [`docker-compose.yml`](docker-compose.yml) publishes that port on `8080` of the host
  machine (`ports: "8080:8080"`).

## Health checks

The API exposes two health endpoints, following the standard **liveness / readiness** split:

| Endpoint | Question it answers | Checks Redis? | Status codes |
|----------|---------------------|---------------|--------------|
| `GET /health/live` | Is the process running and responsive? | No | `200 Healthy` |
| `GET /health/ready` | Can the app actually serve traffic (Redis reachable)? | Yes | `200 Healthy` / `503 Unhealthy` |

The split lets an orchestrator (like kubernetes) react appropriately: a failing **liveness** probe means *restart the
container*, while a failing **readiness** probe means *keep the container but stop routing traffic to
it* until its dependencies recover.

### How it works: background polling + cache

The health check is **not** evaluated when an endpoint is called. Instead it runs **in the background
every 10 seconds**, and the endpoints return the last cached result. This keeps the endpoints
instantaneous (~1 ms) and bounds the load on Redis regardless of how often the probes are called.

A consequence of background polling: a change in Redis availability takes **up to 10 s** to be
reflected by the endpoints. Lower `Period` for faster detection at the cost of more frequent pings.

### Docker / Compose probe

[`docker-compose.yml`](docker-compose.yml) defines a container health check that calls
`/health/ready` (using `curl`, installed in the runtime image for this purpose):

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health/ready"]
  interval: 10s
  timeout: 3s
  retries: 3
  start_period: 10s
```
