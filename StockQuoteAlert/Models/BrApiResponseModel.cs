using System.Text.Json.Serialization;

namespace StockQuoteAlert.Models;

public record BrApiResponseModel(
    [property: JsonPropertyName("results")] StockInfo[] Results,
    [property: JsonPropertyName("requestedAt")] DateTime RequestedAt,
    [property: JsonPropertyName("took")] int Took
);

public record StockInfo(
    [property: JsonPropertyName("symbol")] string Symbol,
    [property: JsonPropertyName("longName")] string LongName,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("regularMarketPrice")] decimal RegularMarketPrice,
    [property: JsonPropertyName("logourl")] string LogoUrl
);
