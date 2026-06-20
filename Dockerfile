# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY FizzBuzz/FizzBuzz.csproj FizzBuzz/
RUN dotnet restore FizzBuzz/FizzBuzz.csproj

COPY FizzBuzz/ FizzBuzz/
RUN dotnet publish FizzBuzz/FizzBuzz.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FizzBuzz.dll"]
