using System.Collections.Generic;
using System.Linq;

namespace Tcgv.CryptoCorrelation.App.Model
{
    public class Router
    {
        public Router()
        {
            pairs = new HashSet<IDexPair>();
        }

        internal void AddPair(DexPair p)
        {
            pairs.Add(p);
            pairs.Add(new InvertedDexPair(p));
        }

        internal IEnumerable<IDexPair> GetPairs()
        {
            return pairs.Where(p => p is DexPair);
        }

        internal IEnumerable<Path> GetCycles()
        {
            foreach (var p in pairs)
            {
                var found = new List<List<IDexPair>>();
                FindCycle(
                    found,
                    new LinkedList<IDexPair>(new[] { p })
                );

                foreach (var cycle in found)
                    yield return new Path(cycle);
            }
        }

        private void FindCycle(
            List<List<IDexPair>> found,
            LinkedList<IDexPair> curr
        )
        {
            foreach (var p in pairs.Where(x => !curr.Contains(x)))
            {
                if (p.A == curr.Last.Value.B && p.Root != curr.Last.Value.Root)
                {
                    curr.AddLast(p);

                    if (curr.First.Value.A == p.B)
                        found.Add(curr.ToList());
                    else
                        FindCycle(found, curr);

                    curr.RemoveLast();
                }
            }
        }

        private HashSet<IDexPair> pairs;
    }
}
