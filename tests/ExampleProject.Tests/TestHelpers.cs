using System;
using System.Collections.Generic;
using Xunit;

namespace NearestPrimeNumber.Tests
{
    public static class TestHelpers
    {
        public static void AssertResultMatchesExpectation(
            long input, 
            bool expected, 
            bool result
            )
        {
            Assert.True(
                expected==result,
                $"Input: {input}, Expected: {expected}, Result: {result}"
            );
        }

        public static void AssertResultMatchesExpectation(
            long expected, 
            long result
            )
        {
            Assert.True(
                expected==result,
                $"Expected: {expected}, Result: {result}"
            );
        }
    }
}
