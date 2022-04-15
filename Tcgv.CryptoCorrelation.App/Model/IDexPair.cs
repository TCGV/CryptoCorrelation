namespace Tcgv.CryptoCorrelation.App.Model
{
    public interface IDexPair
    {
        Token A { get; }

        Token B { get; }

        IDexPair Root { get; }

        string GetLabel();

        double GetPrice();

        decimal GetAmountOut(Token tokenIn, decimal amountIn);

        decimal Swap(object sender, Token tokenIn, decimal amountIn);
    }
}
