namespace Tcgv.CryptoCorrelation.App.Model
{
    public class ArbitrageBot
    {
        public ArbitrageBot(Router router, Token usd)
        {
            this.router = router;
            this.usd = usd;
        }
        public decimal Minted { get; private set; }

        internal void Run()
        {
            foreach (var cycle in router.GetCycles())
            {
                if (cycle.A == usd)
                {
                    while (cycle.Product() > 1.0)
                    {
                        var range = FindInputRange(cycle);
                        var aIn = OptmizeInputAmount(cycle, range);
                        Mint(aIn);
                        cycle.Swap(this, usd, aIn);
                    }
                }
            }
        }

        private decimal[] FindInputRange(Path cycle)
        {
            var aInMax = 1.0m;
            var aOutMax = cycle.GetAmountOut(usd, aInMax);

            while (true)
            {
                var aIn = 2 * aInMax;
                var aOut = cycle.GetAmountOut(usd, aIn);

                var r1 = aOutMax - aInMax;
                var r2 = aOut - aIn;

                if (r2 > r1)
                {
                    aInMax = aIn;
                    aOutMax = aOut;
                }
                else
                {
                    break;
                }
            };

            return new[] { aInMax / 2, 2 * aInMax };
        }

        private decimal OptmizeInputAmount(Path cycle, decimal[] range)
        {
            var aLIn = range[0];
            var aRIn = range[1];

            do
            {
                var _aLIn = (aRIn - aLIn) / 3 + aLIn;
                var _aRIn = 2 * (aRIn - aLIn) / 3 + aLIn;

                var _aLOut = cycle.GetAmountOut(usd, _aLIn) - _aLIn;
                var _aROut = cycle.GetAmountOut(usd, _aRIn) - _aRIn;

                if (_aROut > _aLOut)
                    aLIn = _aLIn;
                else
                    aRIn = _aRIn;
            }
            while ((aRIn - aLIn) > 1.0m);

            var aIn = (aRIn + aLIn) / 2;
            return aIn;
        }

        private void Mint(decimal amount)
        {
            if (usd.BalanceOf(this) < amount)
            {
                usd.Mint(this, amount);
                Minted += amount;
            }
        }

        private Router router;
        private Token usd;
    }
}
