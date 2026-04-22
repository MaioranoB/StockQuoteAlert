using System.Globalization;

namespace StockQuoteAlert;

public static class Arguments
{
    private static readonly int expectedNumberOfArgs = 3;

    public record ParsedArguments(string Stock, decimal SellPrice, decimal BuyPrice);
    
    public static ParsedArguments ParseCommandLineArguments(string[] args)
    {
        if (args.Length != expectedNumberOfArgs)
            throw new ArgumentException($"Invalid number of arguments! Expected: {expectedNumberOfArgs}, received: {args.Length}");

        if (!decimal.TryParse(args[1].Trim().Replace(",", "."), NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal sellPrice))
            throw new ArgumentException($"Invalid reference price for sale: '{args[1]}'");

        if (!decimal.TryParse(args[2].Trim().Replace(",", "."), NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal buyPrice))
            throw new ArgumentException($"Invalid reference price for purchase: '{args[2]}'");
        
        if (buyPrice >= sellPrice)
            throw new ArgumentException($"Invalid reference prices! Buy price ({buyPrice}) should be less than Sell price ({sellPrice})");

        var stock = args[0];
        // TODO: validar se o mercado está aberto antes de iniciar o monitoramento...

        return new ParsedArguments(stock, sellPrice, buyPrice);
    }
}
