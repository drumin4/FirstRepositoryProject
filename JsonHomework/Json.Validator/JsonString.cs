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
            string inputCopyWithoutQuotes = RemoveQuotes(input);

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

        static string RemoveQuotes(string input)
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

                if (!IsHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsHexDigit(char c)
        {
            const string validHexDigits = "0123456789ABCDEFabcdef";

            return validHexDigits.Contains(c);
        }
    }
}
