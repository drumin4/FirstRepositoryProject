using System;
using System.Linq;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return ContainsOnlyValidNotations(input) && PlacementOfZeroIsValid(input)
                && DotPlacementIsValid(input) && ExponentPlacementIsValid(input);
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

        private static bool PlacementOfZeroIsValid(string input)
        {
            return PlacementOfZeroInPositiveNumberIsValid(input) && PlacementOfZeroInNegativeNumberIsValid(input);
        }

        private static bool PlacementOfZeroInPositiveNumberIsValid(string input)
        {
            return input == "0" || input.StartsWith("0.") || !input.StartsWith('0');
        }

        private static bool PlacementOfZeroInNegativeNumberIsValid(string input)
        {
            return input == "-0" || input.StartsWith("-0.") || (!input.StartsWith('-') || input[1] != '0');
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
            const string plusMinus = "-+";
            int exponentIndex = input.ToLower().IndexOf('e');

            return input.ToLower().Substring(exponentIndex).Length > 1 && plusMinus.Contains(input[exponentIndex + 1])
                && input.ToLower().Substring(exponentIndex + 1).Length > 1;
        }
    }
}
