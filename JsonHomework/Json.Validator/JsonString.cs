using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return CheckForControlCharacter(input) && IsWrappedInDoubleQuotes(input) && !ContainsUnrecognizedEscapeCharacters(input);
        }

        private static bool CheckForControlCharacter(string input)
        {
            const int deleteControlCharacter = 127;

            if (IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (IsControlCharacter(c) || deleteControlCharacter.Equals(c))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsControlCharacter(char c)
        {
            return c < ' ';
        }

        private static bool IsWrappedInDoubleQuotes(string input)
        {
            if (IsNullOrEmpty(input))
            {
                return false;
            }

            return input[0] == '"' && input[input.Length - 1] == '"';
        }

        private static bool IsNullOrEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }

        static bool ContainsUnrecognizedEscapeCharacters(string input)
        {
            if (EndsWithReverseSolidus(input))
            {
                return true;
            }

            for (int i = 1; i < input.Length - 1; i++)
            {
                if (input[i] == '\\')
                {
                    if (!IsValidEscapeSequence(input[i + 1], input, i + 1))
                    {
                        return true;
                    }

                    i++;
                }
            }

            return false;
        }

        static bool EndsWithReverseSolidus(string input)
        {
            const int excludeEndingQuotes = 2;

            return input[input.Length - excludeEndingQuotes] == '\\';
        }

        static bool IsValidEscapeSequence(char c, string input, int position)
        {
            const string charactersFromValidEscapeSequences = "\\\"ntrbf\'/";

            if (CheckForUnicodeEscapeSequences(c, input, position))
            {
                return true;
            }

            for (int i = 0; i < charactersFromValidEscapeSequences.Length; i++)
            {
                if (charactersFromValidEscapeSequences[i] == c)
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckForUnicodeEscapeSequences(char c, string input, int position)
        {
            const int excludeEndingQuotes = 2;
            const int numberOfDigitsNeeded = 4;

            if (c != 'u')
            {
                return false;
            }
            else if (input.Substring(position + 1).Length - excludeEndingQuotes < numberOfDigitsNeeded)
            {
                return false;
            }

            return TryParseUnicodeEscapeSequence(input.Substring(position + 1, numberOfDigitsNeeded));
        }

        static bool TryParseUnicodeEscapeSequence(string input)
        {
            string unicodeEscapeSequence = "\\u" + input;
            string unescaped = Regex.Unescape(unicodeEscapeSequence);

            return unicodeEscapeSequence != unescaped;
        }
    }
}
