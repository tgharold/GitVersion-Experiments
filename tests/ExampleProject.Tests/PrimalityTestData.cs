using System;
using System.Collections.Generic;
using Xunit;

namespace NearestPrimeNumber.Tests
{
    public class PrimalityTestData
    {
        ///<summary>Test data below 30,000.</summary>
        public static IReadOnlyCollection<object[]> TinyPrimes => new []
        {
            new object[] { 1, true },
            new object[] { 2, true },
            new object[] { 3, true },
            new object[] { 4, false },
            new object[] { 5, true },
            new object[] { 6, false },
            new object[] { 7, true },
            new object[] { 8, false },
            new object[] { 9, false },
            new object[] { 10, false },
            new object[] { 11, true },
            new object[] { 12, false },
            new object[] { 13, true },
            new object[] { 14, false },
            new object[] { 15, false },
            new object[] { 16, false },
            new object[] { 17, true },
            new object[] { 18, false },
            new object[] { 19, true },
            new object[] { 20, false },
            new object[] { 193, true },
            new object[] { 194, false },
            new object[] { 195, false },
            new object[] { 196, false },
            new object[] { 197, true },
            new object[] { 198, false },
            new object[] { 199, true },
            new object[] { 200, false },
            new object[] { 5001, false },
            new object[] { 5002, false },
            new object[] { 5003, true },
            new object[] { 5004, false },
            new object[] { 5005, false },
            new object[] { 5006, false },
            new object[] { 5007, false },
            new object[] { 5008, false },
            new object[] { 5009, true },
            new object[] { 5010, false },
            new object[] { 5011, true },
            new object[] { 5012, false },
            new object[] { 5013, false },
            new object[] { 5014, false },
            new object[] { 5015, false },
            new object[] { 5016, false },
            new object[] { 5017, false },
            new object[] { 5018, false },
            new object[] { 5019, false },
            new object[] { 5020, false },
            new object[] { 5021, true },
            new object[] { 5022, false },
            new object[] { 5023, true },
            new object[] { 5024, false },
            new object[] { 5025, false },
            new object[] { 5026, false },
            new object[] { 5027, false },
            new object[] { 5028, false },
            new object[] { 5029, false },
            new object[] { 5030, false },
        };

        ///<summary>Primes from 30,001 to 16 million</summary>
        public static IReadOnlyCollection<object[]> SmallPrimes => new []
        {
            new object[] { 40063, true },
            new object[] { 40064, false },
            new object[] { 40065, false },
            new object[] { 40066, false },
            new object[] { 40087, true },
            new object[] { 598579, false },
            new object[] { 598613, true },
            new object[] { 598641, false },
            new object[] { 15484827, false },
            new object[] { 15484873, true },
            new object[] { 15484875, false },
        };
    }
}
