using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        private static string escapableCharacters = "\\\"ntrbf/";

        public static bool IsJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return !ContainsControlCharacters(input) && IsWrappedInDoubleQuotes(input) && !ContainsUnrecognizedEscapeCharacters(input);
        }

        private static bool ContainsControlCharacters(string input)
        {
            foreach (char c in input)
            {
                if (c < ' ')
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsWrappedInDoubleQuotes(string input)
        {
            return input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"';
        }

        static bool ContainsUnrecognizedEscapeCharacters(string input)
        {
            string checkupInputCopy = input[1..^1];

            while (checkupInputCopy.Length > 0)
            {
                if (checkupInputCopy.StartsWith('\\'))
                {
                    const int escapeSequenceLength = 2;
                    const int unicodeEscapeSequenceLength = 6;

                    if (!CurrentEscapeSequenceIsValid(checkupInputCopy))
                    {
                        return true;
                    }

                    checkupInputCopy = escapableCharacters.Contains(checkupInputCopy[1]) ?
                        checkupInputCopy[escapeSequenceLength..] : checkupInputCopy[unicodeEscapeSequenceLength..];
                }
                else
                {
                    checkupInputCopy = checkupInputCopy[1..];
                }
            }

            return false;
        }

        static bool CurrentEscapeSequenceIsValid(string input)
        {
            const int minimumLengthForValidEscapeSequence = 2;

            if (input.Length < minimumLengthForValidEscapeSequence)
            {
                return false;
            }

            char characterAfterReverseSolidus = input[1];

            return escapableCharacters.Contains(characterAfterReverseSolidus) || IsAUnicodeEscapeSequence(input);
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

                if (!IsValidHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsValidHexDigit(char c)
        {
            return IsNumericHexDigit(c) || IsAlphabeticHexDigit(c);
        }

        static bool IsNumericHexDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        static bool IsAlphabeticHexDigit(char c)
        {
            return (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }
    }
}
