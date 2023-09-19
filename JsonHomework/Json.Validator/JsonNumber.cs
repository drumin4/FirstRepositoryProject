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
            if (integralPartOfInput[0] == '-' || integralPartOfInput[0] == '+')
            {
                return ContainsValidDigits(integralPartOfInput[1..], true) && PlacementOfZeroIsValid(integralPartOfInput[1..]);
            }

            return ContainsValidDigits(integralPartOfInput, true) && PlacementOfZeroIsValid(integralPartOfInput);
        }

        private static bool FractionalPartIsValid(string fractionalPartOfInput)
        {
            if (fractionalPartOfInput == null)
            {
                return true;
            }

            if (fractionalPartOfInput.Length == 1)
            {
                return false;
            }

            return ContainsValidDigits(fractionalPartOfInput[1..], false);
        }

        private static bool ExponentialPartIsValid(string exponentialPartOfInput)
        {
            if (exponentialPartOfInput == null)
            {
                return true;
            }

            if (exponentialPartOfInput.Length == 1 || exponentialPartOfInput.EndsWith('+') || exponentialPartOfInput.EndsWith('-'))
            {
                return false;
            }

            if (exponentialPartOfInput[1] == '-' || exponentialPartOfInput[1] == '+')
            {
                const int startingIndexDigits = 2;

                return ContainsValidDigits(exponentialPartOfInput[startingIndexDigits..], true) && PlacementOfZeroIsValid(exponentialPartOfInput[startingIndexDigits..]);
            }

            return ContainsValidDigits(exponentialPartOfInput[1..], true) && PlacementOfZeroIsValid(exponentialPartOfInput[1..]);
        }

        private static bool ContainsValidDigits(string input, bool integralOrExponential)
        {
            foreach (char c in input)
            {
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

            if (exponentIndex != -1)
            {
                return input[dotIndex..exponentIndex];
            }

            return input[dotIndex..];
        }

        private static string ExtractExponent(string input, int exponentIndex)
        {
            return exponentIndex != -1 ? input[exponentIndex..] : null;
        }
    }
}
