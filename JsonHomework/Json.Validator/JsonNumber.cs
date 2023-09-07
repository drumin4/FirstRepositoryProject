using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return double.TryParse(input, out double dummyResult) && !StartsWithZero(input) && !EndsWithDot(input);
        }

        private static bool StartsWithZero(string input)
        {
            int positionOfZero = input[0] == '-' ? 1 : 0;

            return input[positionOfZero] == '0' && (input.Length > positionOfZero + 1 && input[positionOfZero + 1] != '.');
        }

        private static bool EndsWithDot(string input)
        {
            return input[^1] == '.';
        }
    }
}
