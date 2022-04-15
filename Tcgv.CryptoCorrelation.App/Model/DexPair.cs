namespace Tcgv.CryptoCorrelation.App.Model
{
    public class DexPair : IDexPair
    {
        public DexPair(Token a, Token b)
        {
            A = a;
            B = b;
        }

        public Token A { get; }

        public Token B { get; }

        public IDexPair Root { get { return this; } }

        public string GetLabel()
        {
            return $"{A.Symbol}/{B.Symbol}";
        }

        public double GetPrice()
        {
            var qA = (double)A.BalanceOf(this);
            var qB = (double)B.BalanceOf(this);
            return qB / qA;
        }

        public decimal GetAmountOut(Token tokenIn, decimal amountIn)
        {
            var qIn = (double)(tokenIn == A ? A : B).BalanceOf(this);
            var qOut = (double)(tokenIn == A ? B : A).BalanceOf(this);
            var amountOut = (decimal)(qOut - qIn * qOut / (qIn + (double)amountIn));
            return amountOut;
        }

        public decimal Swap(object sender, Token tokenIn, decimal amountIn)
        {
            var amountOut = GetAmountOut(tokenIn, amountIn);
            (tokenIn == A ? A : B).Transfer(sender, this, amountIn);
            (tokenIn == A ? B : A).Transfer(this, sender, amountOut);
            return amountOut;
        }
    }
}
