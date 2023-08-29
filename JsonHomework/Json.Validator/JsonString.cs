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
                if (IsControlCharacter(c) || c == deleteControlCharacter)
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
            char[] charactersForValidEscapeSequences = { '\\', '\"', 'n', 't', 'r', '0', 'a', 'b', 'f', 'v', '\'', '/' };

            if (CheckForUnicodeAndHexadecimalEscapeSequences(c, input, position))
            {
                return true;
            }

            foreach (char character in charactersForValidEscapeSequences)
            {
                if (character == c)
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckForUnicodeAndHexadecimalEscapeSequences(char c, string input, int position)
        {
            const int excludeEndingQuotes = 2;

            Dictionary<char, int> keyValuePairs = new Dictionary<char, int>
        {
            { 'x', 2 },
            { 'u', 4 },
            { 'U', 8 }
        };

            foreach (char character in keyValuePairs.Keys)
            {
                if (character == c)
                {
                    return input.Substring(position).Length - excludeEndingQuotes >= keyValuePairs[character] && !input.Substring(position, keyValuePairs[character]).Contains(' ');
                }
            }

            return false;
        }
    }
}
