namespace StockQuoteAlert.Settings;

public record EmailSettings(
    string FromName,
    string FromAddress,
    string RecipientAddress
);
