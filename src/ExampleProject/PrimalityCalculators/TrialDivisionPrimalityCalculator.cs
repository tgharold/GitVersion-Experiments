using System;

namespace NearestPrimeNumber.PrimalityCalculators
{
    ///<summary>Brute-force approach to testing for primality.</summary>
    public class TrialDivisionPrimalityCalculator : IPrimalityCalculator
    {
        // https://en.wikipedia.org/wiki/Trial_division
        public bool IsPrime(long input)
        {
            if (input < 1) throw new ArgumentOutOfRangeException(nameof(input), "Must be > 0");
            if (input > Int32.MaxValue)  throw new ArgumentOutOfRangeException(nameof(input), $"Must be <= {Int32.MaxValue}");
            if (input <= 3) return true; // handles the trivial cases
            if (input % 2 == 0) return false; // compilers can bit-shift to calculate this, really fast

            var sqrt = (int) Math.Floor(Math.Sqrt(input));
            for (var i = 3; i <= sqrt; i++)
            {
                if (input % i == 0) return false;
            }

            return true;
        }
    }
}
