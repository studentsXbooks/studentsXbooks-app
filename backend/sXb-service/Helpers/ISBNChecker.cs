using System.Linq;
using System;
namespace sXb_service
{
    public class ISBNChecker
    {
        private static int ISBN10RequiredDigitLength => 10;
        private static int ISBN10ModCheck => 11;

        // If each number multiplied by it's index summed is a multiple of 11, return true
        public static bool isISBN10(string maybeISBN)
        {
            var cleanedString = String.Join("", maybeISBN.Trim().Split('-'));
            if (cleanedString.Count() != ISBN10RequiredDigitLength) return false;
            var numberSum = cleanedString
            // Start by group numbers by indexes in a Tuple, Can't get index in Aggregate
            .Select((c, index) =>
            {
                if (c == 'x' || c == 'X') return Tuple.Create(10, index);
                else return Tuple.Create(Int32.Parse(c.ToString()), index);
            })
            .Aggregate(0, (acc, curr) =>
            {
                var value = curr.Item1;
                var index = curr.Item2;
                var result = value * (index + 1);
                return result + acc;
            });
            return (numberSum % ISBN10ModCheck == 0);
        }

        public static bool isISBN13(string value)
        {
            throw new NotImplementedException();
        }
    }
}