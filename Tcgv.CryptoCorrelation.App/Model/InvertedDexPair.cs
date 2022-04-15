namespace Tcgv.CryptoCorrelation.App.Model
{
    public class InvertedDexPair : IDexPair
    {
        public InvertedDexPair(IDexPair pair)
        {
            this.pair = pair;
        }

        public Token A { get { return pair.B; } }

        public Token B { get { return pair.A; } }

        public IDexPair Root { get { return pair.Root; } }

        public string GetLabel()
        {
            return $"{A.Symbol}/{B.Symbol}";
        }

        public double GetPrice()
        {
            return 1 / pair.GetPrice();
        }

        public decimal GetAmountOut(Token tokenIn, decimal amountIn)
        {
            return pair.GetAmountOut(tokenIn, amountIn);
        }

        public decimal Swap(object sender, Token tokenIn, decimal amountIn)
        {
            return pair.Swap(sender, tokenIn, amountIn);
        }

        private IDexPair pair;
    }
}
