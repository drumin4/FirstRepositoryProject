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

            return IntegralPartIsValid(ExtractInteger(input)) && FractionalPartIsValid(ExtractFraction(input))
                && ExponentialPartIsValid(ExtractExponent(input));
        }

        private static bool IntegralPartIsValid(string integralPartOfInput)
        {
            return ContainsOnlyValidNotations(integralPartOfInput, true) && PlacementOfZeroIsValid(integralPartOfInput);
        }

        private static bool FractionalPartIsValid(string fractionalPartOfInput)
        {
            if (fractionalPartOfInput == "number doesn't contain a fraction")
            {
                return true;
            }

            if (string.IsNullOrEmpty(fractionalPartOfInput))
            {
                return false;
            }

            return ContainsOnlyValidNotations(fractionalPartOfInput, false);
        }

        private static bool ExponentialPartIsValid(string exponentialPartOfInput)
        {
            if (exponentialPartOfInput == "number doesn't contain an exponent")
            {
                return true;
            }

            if (string.IsNullOrEmpty(exponentialPartOfInput) || exponentialPartOfInput.EndsWith('+') || exponentialPartOfInput.EndsWith('-'))
            {
                return false;
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

        private static bool PlacementOfZeroIsValid(string input)
        {
           return (!input.StartsWith("0") && !input.StartsWith("-0") && !input.StartsWith("+0"))
                   || input.EndsWith("0");
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
            if (input.Contains("."))
            {
                if (input.ToLower().Contains('e'))
                {
                    int lengthFraction = input.ToLower().IndexOf('e') - (input.IndexOf('.') + 1);

                    return input.Substring(input.IndexOf('.') + 1, lengthFraction);
                }

                return input.Substring(input.IndexOf('.') + 1);
            }

            return "number doesn't contain a fraction";
        }

        private static string ExtractExponent(string input)
        {
            if (input.ToLower().Contains("e"))
            {
                return input.Substring(input.ToLower().IndexOf('e') + 1);
            }

            return "number doesn't contain an exponent";
        }
    }
}
