using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return double.TryParse(input, out double result) && !StartsWithZero(input) && !EndsWithDot(input);
        }

        private static bool StartsWithZero(string input)
        {
            return input[0] == '0' && (!input.Contains('.') && input.Length != 1);
        }

        private static bool EndsWithDot(string input)
        {
            return input[input.Length - 1] == '.';
        }
    }
}
