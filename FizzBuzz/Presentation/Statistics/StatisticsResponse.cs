namespace FizzBuzz.Presentation.Statistics;

public record StatisticsResponse(StatisticsRequestResponse Request, int Hits);

public record StatisticsRequestResponse(int Int1, int Int2, int Limit, string Str1, string Str2);
