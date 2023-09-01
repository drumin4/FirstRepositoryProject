using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return CheckForControlCharacter(input) && IsWrappedInDoubleQuotes(input) && !ContainsUnrecognizedEscapeCharacters(input);
        }

        private static bool CheckForControlCharacter(string input)
        {
            const int deleteControlCharacter = 127;

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
            if (input.Length < 2)
            {
                return false;
            }

            return input[0] == '"' && input[input.Length - 1] == '"';
        }

        static bool ContainsUnrecognizedEscapeCharacters(string input)
        {
            string inputCopy = input;

            if (EndsWithReverseSolidus(input))
            {
                return true;
            }

            const int removeTheFirstTwoCharacters = 3;
            const int removeTheFirstCharacter = 2;
            const int charAfterReverseSolidusPosition = 2;

            while (inputCopy.Length > 2)
            {
                if (inputCopy[1] == '\\')
                {
                    if (!IsValidEscapeSequence(inputCopy[charAfterReverseSolidusPosition], inputCopy, charAfterReverseSolidusPosition))
                    {
                        return true;
                    }

                    inputCopy = inputCopy[0] + inputCopy[removeTheFirstTwoCharacters..];
                }
                else
                {
                    inputCopy = inputCopy[0] + inputCopy[removeTheFirstCharacter..];
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
            int startingIndexOfUnicodeDigits = position + 1;
            int endingIndexOfUnicodeDigits = position + 1 + numberOfDigitsNeeded;

            if (c != 'u' || input[startingIndexOfUnicodeDigits..].Length - excludeEndingQuotes < numberOfDigitsNeeded)
            {
                return false;
            }

            return VerifyUnicodeEscapeSequence(input[startingIndexOfUnicodeDigits..endingIndexOfUnicodeDigits]);
        }

        static bool VerifyUnicodeEscapeSequence(string input)
        {
            string unicodeEscapeSequence = "\\u" + input;
            string unescaped = Regex.Unescape(unicodeEscapeSequence);

            return unicodeEscapeSequence != unescaped;
        }
    }
}
