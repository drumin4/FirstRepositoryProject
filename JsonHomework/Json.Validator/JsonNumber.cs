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

            int dotIndex = input.IndexOf('.');
            int exponentIndex = input.ToLower().IndexOf('e');

            return IntegralPartIsValid(ExtractInteger(input, dotIndex, exponentIndex))
                && FractionalPartIsValid(ExtractFraction(input, dotIndex, exponentIndex))
                && ExponentialPartIsValid(ExtractExponent(input, exponentIndex));
        }

        private static bool IntegralPartIsValid(string integralPartOfInput)
        {
            return ContainsValidDigits(integralPartOfInput, true) && PlacementOfZeroIsValid(integralPartOfInput);
        }

        private static bool FractionalPartIsValid(string fractionalPartOfInput)
        {
            if (fractionalPartOfInput == null)
            {
                return true;
            }

            if (fractionalPartOfInput == "")
            {
                return false;
            }

            return ContainsValidDigits(fractionalPartOfInput, false);
        }

        private static bool ExponentialPartIsValid(string exponentialPartOfInput)
        {
            if (exponentialPartOfInput == null)
            {
                return true;
            }

            if (exponentialPartOfInput == "" || exponentialPartOfInput.EndsWith('+') || exponentialPartOfInput.EndsWith('-'))
            {
                return false;
            }

            return ContainsValidDigits(exponentialPartOfInput, true) && PlacementOfZeroIsValid(exponentialPartOfInput);
        }

        private static bool ContainsValidDigits(string input, bool integralOrExponential)
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

        private static string ExtractInteger(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex != -1)
            {
                return input[0..dotIndex];
            }
            else if (exponentIndex != -1)
            {
                return input[0..exponentIndex];
            }

            return input;
        }

        private static string ExtractFraction(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex == -1)
            {
                return null;
            }

            int fractionStartingIndex = dotIndex + 1;

            if (exponentIndex != -1)
            {
                return input[fractionStartingIndex..exponentIndex];
            }

            return input[fractionStartingIndex..];
        }

        private static string ExtractExponent(string input, int exponentIndex)
        {
            int exponentStartingIndex = exponentIndex + 1;

            return exponentIndex != -1 ? input[exponentStartingIndex..] : null;
        }
    }
}
