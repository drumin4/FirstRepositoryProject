using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        private static string charactersFromValidEscapeSequences = "\\\"ntrbf/";

        public static bool IsJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return IsFreeOfControlCharacters(input) && IsWrappedInDoubleQuotes(input) && !ContainsUnrecognizedEscapeCharacters(input);
        }

        private static bool IsFreeOfControlCharacters(string input)
        {
            const string controlCharacters = "\n\r\t\f\b";

            foreach (char c in input)
            {
                if (controlCharacters.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsWrappedInDoubleQuotes(string input)
        {
            if (input.Length < 2)
            {
                return false;
            }

            return input[0] == '"' && input[input.Length - 1] == '"';
        }

        static bool ContainsUnrecognizedEscapeCharacters(string input)
        {
            if (IsComposedOfAQuotedEmptyString(input))
            {
                return false;
            }

            string inputCopyWithoutQuotes = RemoveQuotes(input);

            if (EndsWithAnUnescapedReverseSolidus(inputCopyWithoutQuotes))
            {
                return true;
            }

            while (inputCopyWithoutQuotes.Length > 0)
            {
                if (inputCopyWithoutQuotes[0] == '\\' && !IsValidEscapeSequence(inputCopyWithoutQuotes))
                {
                    return true;
                }
                else if (inputCopyWithoutQuotes[0] == '\\' && IsValidEscapeSequence(inputCopyWithoutQuotes))
                {
                    inputCopyWithoutQuotes = RemoveFormerEscapeSequence(inputCopyWithoutQuotes);
                }
                else
                {
                    inputCopyWithoutQuotes = inputCopyWithoutQuotes[1..];
                }
            }

            return false;
        }

        static string RemoveFormerEscapeSequence(string input)
        {
            const int escapeSequenceLength = 2;
            const int unicodeEscapeSequenceLength = 6;

            return charactersFromValidEscapeSequences.Contains(input[1]) ? input[escapeSequenceLength..] : input[unicodeEscapeSequenceLength..];
        }

        static string RemoveQuotes(string input)
        {
            int lastPosition = input.Length - 1;

            if (input[0] == '"' && input[lastPosition] == '"')
            {
                return input[1..lastPosition];
            }

            return input;
        }

        static bool IsComposedOfAQuotedEmptyString(string input)
        {
            return input.Length == 2;
        }

        static bool EndsWithAnUnescapedReverseSolidus(string input)
        {
            int penultimatePosition = input.Length - 2;

            return input[input.Length - 1] == '\\' && input[penultimatePosition] != '\\';
        }

        static bool IsValidEscapeSequence(string input)
        {
            char characterAfterReverseSolidus = input[1];

            return charactersFromValidEscapeSequences.Contains(characterAfterReverseSolidus) || IsAUnicodeEscapeSequence(input);
        }

        static bool IsAUnicodeEscapeSequence(string input)
        {
            const int validUnicodeEscapeSequenceLength = 6;

            if (!input.StartsWith("\\u") || input.Length < validUnicodeEscapeSequenceLength)
            {
                return false;
            }

            for (int i = 2; i < validUnicodeEscapeSequenceLength; i++)
            {
                char c = input[i];

                if (!IsHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsHexDigit(char c)
        {
            const string validNumbers = "0123456789";
            const string validUppercaseLetters = "ABCDEF";
            const string validLowercaseLetters = "abcdef";

            return validNumbers.Contains(c) || validUppercaseLetters.Contains(c) || validLowercaseLetters.Contains(c);
        }
    }
}
