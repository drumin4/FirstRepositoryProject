using System;
using System.Linq;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return !StartsWithZero(input) && DotPlacementIsValid(input) && ExponentPlacementIsValid(input);
        }

        private static bool DotPlacementIsValid(string input)
        {
            return !EndsWithDot(input) && HasNoMoreThanOneDot(input);
        }

        private static bool StartsWithZero(string input)
        {
            int positionOfZero = input[0] == '-' ? 1 : 0;

            return input[positionOfZero] == '0' && (input.Length > positionOfZero + 1 && input[positionOfZero + 1] != '.');
        }

        private static bool HasNoMoreThanOneDot(string input)
        {
            return input.IndexOf(".") == input.LastIndexOf(".");
        }

        private static bool EndsWithDot(string input)
        {
            return input[^1] == '.';
        }

        private static bool ExponentPlacementIsValid(string input)
        {
            return HasNoMoreThanOneExponent(input);
        }

        private static bool HasNoMoreThanOneExponent(string input)
        {
            return input.ToLower().IndexOf('e') == input.ToLower().LastIndexOf('e');
        }
    }
}
