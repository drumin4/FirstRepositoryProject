using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return IntegerPartIsValid(ExtractInteger(input)) && FractionalPartIsValid(ExtractFraction(input))
                && ExponentPartIsValid(ExtractExponent(input));

            //return ContainsOnlyValidNotations(input) && PlacementOfZeroIsValid(input)
            //    && DotPlacementIsValid(input) && ExponentPlacementIsValid(input);
        }

        private static bool IntegerPartIsValid(string input)
        {

        }

        private static bool FractionalPartIsValid(string input)
        {
            if (input == null)
            {
                return true;
            }
        }

        private static bool ExponentPartIsValid(string input)
        {
            if (input == null)
            {
                return true;
            }
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
            return !input.ToLower().EndsWith('e') && !input.EndsWith('+') && !input.EndsWith('-');
        }

        private static string ExtractInteger(string input)
        {
            if (input.Contains("."))
            {
                return input.Substring(0, input.IndexOf('.'));
            }
            else if (input.ToLower().Contains('e'))
            {
                return input.Substring(0, input.ToLower().IndexOf('e'));
            }

            return input;
        }

        private static string ExtractFraction(string input)
        {
            if (!input.Contains("."))
            {
                return null;
            }

            if (input.ToLower().Contains('e'))
            {
                return input.Substring(input.ToLower().IndexOf('.') + 1, input.ToLower().IndexOf('e'));
            }

            return input.Substring(input.IndexOf('.') + 1);
        }

        private static string ExtractExponent(string input)
        {
            if (!input.ToLower().Contains("e"))
            {
                return null;
            }

            return input.Substring(input.ToLower().IndexOf('e') + 1);
        }

        private static bool CheckNumberParts(string input)
        {
            foreach (char ch in input)
            {
                if (ch < '0' || ch > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
