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
            if (integralPartOfInput[0] == '-')
            {
                integralPartOfInput = integralPartOfInput[1..];
            }

            return ContainsValidDigits(integralPartOfInput) && PlacementOfZeroIsValid(integralPartOfInput);
        }

        private static bool FractionalPartIsValid(string fractionalPartOfInput)
        {
            return fractionalPartOfInput == "" || ContainsValidDigits(fractionalPartOfInput[1..]);
        }

        private static bool ExponentialPartIsValid(string exponentialPartOfInput)
        {
            if (exponentialPartOfInput == "")
            {
                return true;
            }

            exponentialPartOfInput = exponentialPartOfInput[1..];

            if (exponentialPartOfInput.StartsWith('-') || exponentialPartOfInput.StartsWith('+'))
            {
                exponentialPartOfInput = exponentialPartOfInput[1..];
            }

            return ContainsValidDigits(exponentialPartOfInput);
        }

        private static bool ContainsValidDigits(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return input.Length > 0;
        }

        private static bool PlacementOfZeroIsValid(string input)
        {
           return !input.StartsWith("0") || input.LastIndexOf('0') == 0 && input.EndsWith('0');
        }

        private static string ExtractInteger(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex != -1)
            {
                return input[..dotIndex];
            }

            if (exponentIndex != -1)
            {
                return input[..exponentIndex];
            }

            return input;
        }

        private static string ExtractFraction(string input, int dotIndex, int exponentIndex)
        {
            if (dotIndex == -1)
            {
                return "";
            }

            if (exponentIndex != -1)
            {
                return input[dotIndex..exponentIndex];
            }

            return input[dotIndex..];
        }

        private static string ExtractExponent(string input, int exponentIndex)
        {
            return exponentIndex != -1 ? input[exponentIndex..] : "";
        }
    }
}
