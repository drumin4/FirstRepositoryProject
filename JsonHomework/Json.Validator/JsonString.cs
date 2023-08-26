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
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\\')
                {
                    if (i + 1 == input.Length - 1 || !IsValidEscapeSequence(input[i + 1], input, i + 1))
                    {
                        return true;
                    }

                    i++;
                }
            }

            return false;
        }

        static bool IsValidEscapeSequence(char c, string input, int position)
        {
            return CheckFirstSetOfValidEscapeSequences(c) || CheckSecondSetOfValidEscapeSequences(c) || CheckThirdSetOfValidEscapeSequences(c, input, position);
        }

        static bool CheckFirstSetOfValidEscapeSequences(char c)
        {
            return c == '\\' || c == '\"' || c == 'n';
        }

        static bool CheckSecondSetOfValidEscapeSequences(char c)
        {
            return c == 't' || c == 'r' || c == '0';
        }

        static bool CheckThirdSetOfValidEscapeSequences(char c, string input, int position)
        {
            return CheckFourthSetOfValidEscapeSequences(c, input, position) || CheckFifthSetOfValidEscapeSequences(c) || CheckSixthSetOfValidEscapeSequences(c);
        }

        static bool CheckFourthSetOfValidEscapeSequences(char c, string input, int position)
        {
            return CheckUnicodeEscapeSequenceFourDigits(c, input, position) || CheckUnicodeEscapeSequenceEightDigits(c, input, position) || CheckHexadecimalEscapeSequence(c, input, position);
        }

        static bool CheckFifthSetOfValidEscapeSequences(char c)
        {
            return c == 'a' || c == 'b' || c == 'f';
        }

        static bool CheckSixthSetOfValidEscapeSequences(char c)
        {
            return c == 'v' || c == '\'' || c == '/';
        }

        static bool CheckHexadecimalEscapeSequence(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 2;

            return c == 'x' && CheckHowManyFollowingCharacters(input, position) >= numberOfCharactersNeeded && CheckTheFollowingCharactersAreNotSpaces(input, position, numberOfCharactersNeeded);
        }

        static bool CheckUnicodeEscapeSequenceFourDigits(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 4;

            return c == 'u' && CheckHowManyFollowingCharacters(input, position) >= numberOfCharactersNeeded && CheckTheFollowingCharactersAreNotSpaces(input, position, numberOfCharactersNeeded);
        }

        static bool CheckUnicodeEscapeSequenceEightDigits(char c, string input, int position)
        {
            const int numberOfCharactersNeeded = 8;

            return c == 'U' && CheckHowManyFollowingCharacters(input, position) >= numberOfCharactersNeeded && CheckTheFollowingCharactersAreNotSpaces(input, position, numberOfCharactersNeeded);
        }

        static int CheckHowManyFollowingCharacters(string input, int position)
        {
            const int excludeLastDoubleQuotes = 2;

            return input.Length - excludeLastDoubleQuotes - position;
        }

        static bool CheckTheFollowingCharactersAreNotSpaces(string input, int position, int numberOfCharactersNeeded)
        {
            try
            {
                return !input.Substring(position + 1, numberOfCharactersNeeded).Contains(' ');
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}
