using System;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return CheckEachCharacter(input) && IsWrappedInDoubleQuotes(input) && !ContainsUnrecognizedEscapeCharacters(input);
        }

        private static bool CheckEachCharacter(string input)
        {
            const int deleteAsciiCharacter = 127;

            if (IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (IsNonPrintableCharacter(c) || c == deleteAsciiCharacter)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsNonPrintableCharacter(char c)
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
            return CheckUnicodeEscapeSequenceFourDigits(c, input, position) || CheckUnicodeEscapeSequenceEightDigits(c, input, position) || CheckHexadecimalEscapeSequence(c, input, position);
        }

        static bool CheckHexadecimalEscapeSequence(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 2;
            const int excludeEndingQuotes = 2;

            return c == 'x' && input.Substring(position).Length - excludeEndingQuotes >= numberOfCharactersNeeded && !input.Substring(position, numberOfCharactersNeeded).Contains(' ');
        }

        static bool CheckUnicodeEscapeSequenceFourDigits(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 4;
            const int excludeEndingQuotes = 2;

            return c == 'u' && input.Substring(position).Length - excludeEndingQuotes >= numberOfCharactersNeeded && !input.Substring(position, numberOfCharactersNeeded).Contains(' ');
        }

        static bool CheckUnicodeEscapeSequenceEightDigits(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 8;
            const int excludeEndingQuotes = 2;

            return c == 'U' && input.Substring(position).Length - excludeEndingQuotes >= numberOfCharactersNeeded && !input.Substring(position, numberOfCharactersNeeded).Contains(' ');
        }
    }
}
