using System;
using System.Linq;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return ContainsOnlyValidNotations(input) && !StartsWithZero(input) && DotPlacementIsValid(input) && ExponentPlacementIsValid(input);
        }

        private static bool ContainsOnlyValidNotations(string input)
        {
            return !string.IsNullOrEmpty(input) && !ContainsInvalidCharacters(input);
        }

        private static bool ContainsInvalidCharacters(string input)
        {
            const string validCharacters = "eE.-+";

            foreach (char c in input)
            {
                if ((c < '0' || c > '9') && !validCharacters.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool DotPlacementIsValid(string input)
        {
            return input[^1] != '.' && input.IndexOf(".") == input.LastIndexOf(".");
        }

        private static bool StartsWithZero(string input)
        {
            int expectedPositionOfZero = input[0] == '-' ? 1 : 0;

            return input[expectedPositionOfZero] == '0' && (!input.EndsWith('0') && input[expectedPositionOfZero + 1] != '.');
        }

        private static bool ExponentPlacementIsValid(string input)
        {
            return !input.ToLower().Contains("e")
                || (ExponentIsNoMoreThanOne(input) && ExponentIsComplete(input) && ExponentIsAfterTheFraction(input));
        }

        private static bool ExponentIsNoMoreThanOne(string input)
        {
            return input.ToLower().IndexOf('e') == input.ToLower().LastIndexOf('e');
        }

        private static bool ExponentIsAfterTheFraction(string input)
        {
            return input.ToLower().IndexOf('e') == -1 || input.IndexOf('.') < input.ToLower().IndexOf('e');
        }

        private static bool ExponentIsComplete(string input)
        {
            int exponentIndex = input.ToLower().IndexOf('e');
            int positiveSignIndex = input.IndexOf("+");
            int negativeSignIndex = input.IndexOf("-");

            if (positiveSignIndex != -1)
            {
                return !input.EndsWith(input[positiveSignIndex]);
            }
            else if (negativeSignIndex != -1)
            {
                return !input.EndsWith(input[negativeSignIndex]);
            }
            else if (exponentIndex != -1)
            {
                return !input.EndsWith(input[exponentIndex]);
            }

            return true;
        }
    }
}
