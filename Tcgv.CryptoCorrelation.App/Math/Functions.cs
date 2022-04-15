using System.Collections.Generic;
using System.Linq;

namespace Tcgv.CryptoCorrelation.App.Math
{
    public class Functions
    {
        public static double Correlation(List<double> values1, List<double> values2)
        {
            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => System.Math.Pow(x - avg1, 2.0));
            var sumSqr2 = values2.Sum(y => System.Math.Pow(y - avg2, 2.0));

            var result = sum1 / System.Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }
    }
}
