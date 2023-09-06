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
            string inputCopyWithoutQuotes = ExtractTextFromQuotes(input);

            while (inputCopyWithoutQuotes.Length > 0)
            {
                if (inputCopyWithoutQuotes.StartsWith('\\'))
                {
                    if (!CurrentEscapeSequenceIsValid(inputCopyWithoutQuotes))
                    {
                        return true;
                    }

                    inputCopyWithoutQuotes = RemoveCurrentEscapeSequenceFromString(inputCopyWithoutQuotes);
                }
                else if (inputCopyWithoutQuotes.StartsWith('"'))
                {
                    if (inputCopyWithoutQuotes.Length == 1 || inputCopyWithoutQuotes[1] != '"')
                    {
                        return true;
                    }

                    inputCopyWithoutQuotes = RemoveCurrentEscapeSequenceFromString(inputCopyWithoutQuotes);
                }
                else
                {
                    inputCopyWithoutQuotes = RemoveCurrentCharacterFromString(inputCopyWithoutQuotes);
                }
            }

            return false;
        }

        static string RemoveCurrentEscapeSequenceFromString(string input)
        {
            const int escapeSequenceLength = 2;
            const int unicodeEscapeSequenceLength = 6;

            return escapableCharacters.Contains(input[1]) ? input[escapeSequenceLength..] : input[unicodeEscapeSequenceLength..];
        }

        static string RemoveCurrentCharacterFromString(string input)
        {
            return input[1..];
        }

        static string ExtractTextFromQuotes(string input)
        {
            int lastPosition = input.Length - 1;

            return input[1..lastPosition];
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
