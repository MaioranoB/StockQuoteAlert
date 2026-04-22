namespace StockQuoteAlert.Settings;

public record ApplicationSettings(
    SMTPsettings SMTP,
    EmailSettings Email,
    BRAPISettings BRAPI,
    
    int PollingIntervalMs
);
