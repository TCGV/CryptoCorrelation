using System;
using System.Collections.Generic;

namespace Tcgv.CryptoCorrelation.App.Model
{
    public class Path
    {
        public Path(List<IDexPair> path)
        {
            this.path = path;
        }

        public Token A { get { return path[0].A; } }

        public Token B { get { return path[path.Count - 1].B; } }

        public double Product()
        {
            var t = A;
            var prod = 1.0;
            foreach (var p in path)
            {
                prod *= p.A == t ? p.GetPrice() : 1.0 / p.GetPrice();
                t = p.A == t ? p.B : p.A;
            }
            return prod;
        }

        public decimal GetAmountOut(Token tokenIn, decimal amountIn)
        {
            if (tokenIn != A)
                throw new InvalidOperationException();

            foreach (var p in path)
            {
                amountIn = p.GetAmountOut(tokenIn, amountIn);
                tokenIn = p.A == tokenIn ? p.B : p.A;
            }

            return amountIn;
        }

        public decimal Swap(object sender, Token tokenIn, decimal amountIn)
        {
            if (tokenIn != A)
                throw new InvalidOperationException();

            foreach (var p in path)
            {
                amountIn = p.Swap(sender, tokenIn, amountIn);
                tokenIn = p.A == tokenIn ? p.B : p.A;
            }

            return amountIn;
        }


        private List<IDexPair> path;
    }
}
