using System.Diagnostics;

namespace FizzBuzz.Application;

public static class AppDiagnostics
{
    public const string ActivitySourceName = "FizzBuzz.Application";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
