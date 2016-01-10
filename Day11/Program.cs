using System;
using System.Collections.Generic;
using System.Linq;

namespace Day11
{
    class Program
    {
        static char[] InvalidCharacters = { 'i', 'o', 'l' };

        static void Main(string[] args)
        {
            string star1 = CalculateNewPassword("cqjxjnds");

            Console.WriteLine("Day 11 - Star 1, Answer: {0}", star1);

            string star2 = CalculateNewPassword(star1);

            Console.WriteLine("Day 11 - Star 2, Answer: {0}", star2);

            Console.ReadKey();
        }

        static string CalculateNewPassword(string currentPassword)
        {
            string nextPassword = currentPassword;
            int iterations = 0;

            do
            {
                iterations++;
                nextPassword = IncreasePassword(nextPassword);
            } while (
                !HasThreeIncreasingCharacters(nextPassword) ||
                !HasNoInvalidCharacters(nextPassword) ||
                !HasTwoPairs(nextPassword)
            );

            return nextPassword;
        }

        static string IncreasePassword(string password)
        {
            int charPosition = password.Length - 1;

            char nextCharacter;
            bool nextPosition = true;

            do
            {
                nextCharacter = IncreaseChar(password[charPosition]);

                password = password.ReplaceAtIndex(charPosition, nextCharacter);

                nextPosition = (nextCharacter == 'a');

                if (nextPosition)
                {
                    charPosition--;
                }
            } while (nextPosition);

            return password;
        }

        static char IncreaseChar(char character)
        {
            return (char)((((character - 97) + 1) % 26) + 97);
        }

        static bool HasThreeIncreasingCharacters(string password)
        {
            for (int i = 0; i < password.Length - 2; i++)
            {
                if ((password[i] + 2) == (password[i + 1] + 1) && (password[i + 1] + 1) == (password[i + 2]))
                {
                    return true;
                }
            }

            return false;
        }

        static bool HasNoInvalidCharacters(string password)
        {
            return !password.Any(c => InvalidCharacters.Contains(c));
        }

        static List<string> GetPairs()
        {
            List<string> pairs = new List<string>();

            for (int i = 97; i < 97 + 26; i++)
            {
                pairs.Add(((char)i).ToString() + ((char)i).ToString());
            }

            return pairs;
        }

        static bool HasTwoPairs(string password)
        {
            List<string> pairs = GetPairs();

            for (int i = 0; i < password.Length - 1; i++)
            {
                string pair = string.Concat(password[i], password[i + 1]);

                if (pairs.Contains(pair))
                {
                    pairs.Remove(pair);
                }

                if (pairs.Count == 24)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
