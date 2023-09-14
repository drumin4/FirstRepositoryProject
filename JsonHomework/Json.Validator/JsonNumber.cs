using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return IntegerPartIsValid(ExtractInteger(input)) && FractionalPartIsValid(ExtractFraction(input))
                && ExponentialPartIsValid(ExtractExponent(input));
        }

        private static bool IntegerPartIsValid(string integerPartOfInput)
        {
            return ContainsOnlyValidNotations(integerPartOfInput, true) && PlacementOfZeroIsValid(integerPartOfInput);
        }

        private static bool FractionalPartIsValid(string fractionalPartOfInput)
        {
            if (fractionalPartOfInput == null)
            {
                return true;
            }

            return ContainsOnlyValidNotations(fractionalPartOfInput, false);
        }

        private static bool ExponentialPartIsValid(string exponentialPartOfInput)
        {
            if (exponentialPartOfInput == null)
            {
                return true;
            }

            return ContainsOnlyValidNotations(exponentialPartOfInput, true) && PlacementOfZeroIsValid(exponentialPartOfInput);
        }

        private static bool ContainsOnlyValidNotations(string input, bool integralOrExponential)
        {
            foreach (char c in input)
            {
                if (integralOrExponential && input.IndexOf(c) == 0 && (c == '-' || c == '+'))
                {
                    continue;
                }

                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool PlacementOfZeroIsValid(string integerPartOfInput)
        {
           return (!integerPartOfInput.StartsWith("0")
                   && !integerPartOfInput.StartsWith("-0")
                   && !integerPartOfInput.StartsWith("+0"))
                   || integerPartOfInput.Length <= integerPartOfInput.IndexOf('0') + 1;
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
    }
}
