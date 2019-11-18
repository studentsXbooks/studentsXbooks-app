using System.Linq;
using System;
namespace sXb_service
{
    public class ISBNChecker
    {
        private static int ISBN10RequiredDigitLength => 10;
        private static int ISBN10ModCheck => 11;
        private static int ISBN13RequiredDigitLength => 13;

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

        public static bool isISBN13(string maybeISBN)
        {
            var cleanedString = String.Join("", maybeISBN.Trim().Split('-'));
            if (cleanedString.Count() != ISBN13RequiredDigitLength) return false;
            var ISBN13ModCheck = Int32.Parse(cleanedString.Remove(0,12).ToString());

            cleanedString = cleanedString.Remove(12);
            var numberSum = cleanedString.Select((c , index) =>
            {
                return Tuple.Create(Int32.Parse(c.ToString()), index);
            })
            .Aggregate(0, (acc, curr) =>
            {
                var value = curr.Item1;
                var index = curr.Item2;
                if(index % 2 == 0)
                {
                    return value + acc;
                }
                else
                {
                    return value * 3 + acc;
                }
            });
           
            return ISBN13ModCheck == 10 - (numberSum % 10);
        }
    }
}