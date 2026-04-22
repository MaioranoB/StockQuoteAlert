namespace StockQuoteAlert.Settings;

public record SMTPsettings(
    string Server,
    int Port,
    string User,
    string Password
);