using System;
using System.Collections.Generic;
using Tcgv.CryptoCorrelation.App.Math;
using Tcgv.CryptoCorrelation.App.Model;

namespace Tcgv.CryptoCorrelation.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var router = new Router();

            var btc = new Token("BTC");
            var eth = new Token("ETH");
            var usd = new Token("USD");

            // initialize US$ 300M BTC/ETC pair
            var btc_eth_pair = new DexPair(btc, eth);
            btc.Mint(btc_eth_pair, 3333.333m);
            eth.Mint(btc_eth_pair, 46875.000m);
            router.AddPair(btc_eth_pair);

            // initialize US$ 300M BTC/USD pair
            var btc_usd_pair = new DexPair(btc, usd);
            btc.Mint(btc_usd_pair, 3333.333m);
            usd.Mint(btc_usd_pair, 150000000m);
            router.AddPair(btc_usd_pair);

            // initialize US$ 300M ETH/USD pair
            var eth_usd_pair = new DexPair(eth, usd);
            eth.Mint(eth_usd_pair, 46875.000m);
            usd.Mint(eth_usd_pair, 150000000m);
            router.AddPair(eth_usd_pair);

            PrintPairs(router);

            // execute US$ 1M swap
            var swapAmount = 1000000;
            Swap(btc_usd_pair, usd, swapAmount);

            PrintPairs(router);

            // run arbitrage between dex pairs
            var arb = new ArbitrageBot(router, usd);
            arb.Run();

            PrintPairs(router);
        }

        private static void PrintPairs(Router router)
        {
            foreach (var pair in router.GetPairs())
                Console.WriteLine($"{pair.GetLabel()} price = {pair.GetPrice()}");
            Console.WriteLine();
        }

        private static void Swap(DexPair pair, Token tk, decimal amt)
        {
            var trader = new object();
            tk.Mint(trader, amt);
            pair.Swap(trader, tk, amt);
        }
    }
}
