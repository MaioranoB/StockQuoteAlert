using StockQuoteAlert;
using StockQuoteAlert.Services;
using Microsoft.Extensions.Configuration;
using StockQuoteAlert.Settings;
using StockQuoteAlert.Models;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

try
{
    var parsedArguments = Arguments.ParseCommandLineArguments(args);
    var appSettings = configuration.Get<ApplicationSettings>();
    if (appSettings == null)
    {
        Console.Error.WriteLine("Error loading configuration files! (appsettings.json or UserSecrets)");
        return 1;
    }

    BrApiService brApiService = new(appSettings);
    EmailService emailService = new(appSettings);
    AlertType? lastAlertSent = null;
    
    while (true)
    {
        var stockInfo = await brApiService.GetStockQuoteAsync(parsedArguments.Stock);
        if (stockInfo == null) break;

        Console.WriteLine($"{stockInfo.Symbol} MarketPrice: {stockInfo.RegularMarketPrice} | SellPrice: {parsedArguments.SellPrice} | BuyPrice: {parsedArguments.BuyPrice} | LastAlert: {lastAlertSent}\n");

        if (stockInfo.RegularMarketPrice > parsedArguments.SellPrice && lastAlertSent != AlertType.Sell)
        {
            _ = emailService.SendStockAlertEmail(stockInfo, AlertType.Sell, parsedArguments.SellPrice);
            lastAlertSent = AlertType.Sell;
        }
        else if (stockInfo.RegularMarketPrice < parsedArguments.BuyPrice && lastAlertSent != AlertType.Buy)
        {
            _ = emailService.SendStockAlertEmail(stockInfo, AlertType.Buy, parsedArguments.BuyPrice);
            lastAlertSent = AlertType.Buy;
        }

        await Task.Delay(appSettings.PollingIntervalMs);
    }
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
    return 1;
}
