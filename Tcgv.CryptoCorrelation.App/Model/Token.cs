using System;
using System.Collections.Generic;

namespace Tcgv.CryptoCorrelation.App.Model
{
    public class Token
    {
        public Token(string symbol)
        {
            Symbol = symbol;
            balances = new Dictionary<object, decimal>();
        }

        public string Symbol { get; }

        public decimal TotalSupply { get; private set; }

        public decimal BalanceOf(object owner)
        {
            EnsureKey(owner);
            return balances[owner];
        }

        public void Mint(object owner, decimal value)
        {
            EnsureKey(owner);
            balances[owner] += value;
            TotalSupply += value;
        }

        public void Transfer(object from, object to, decimal value)
        {
            EnsureKey(from);
            EnsureKey(to);

            if (balances[from] < value)
                throw new InvalidOperationException();

            balances[from] -= value;
            balances[to] += value;
        }

        private void EnsureKey(object key)
        {
            if (!balances.ContainsKey(key))
                balances.Add(key, 0);
        }

        private Dictionary<object, decimal> balances;
    }
}