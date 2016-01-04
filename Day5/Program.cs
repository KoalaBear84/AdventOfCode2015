using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static List<string> disallowedSubstrings = new List<string> { "ab", "cd", "pq", "xy" };
        static List<char> vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            Console.WriteLine("Day 5 - Star 1, Nice strings: {0}", CheckInputStar1(input));
            Console.WriteLine("Day 5 - Star 2, Nice strings: {0}", CheckInputStar2(input));
            Console.ReadKey();
        }

        static int CheckInputStar1(string[] input)
        {
            return input.Count(ValidWordStar1);
        }

        static bool ValidWordStar1(string input)
        {
            // No disallowed strings
            if (disallowedSubstrings.Any(ds => input.Contains(ds)))
            {
                return false;
            }

            // At least a repeating character, but no vowel
            if (!Regex.IsMatch(input, @"(.)\1{1,}", RegexOptions.IgnoreCase))
            {
                return false;
            }

            // At least three vowels
            if (input.Count(character => vowels.Contains(character)) < 3)
            {
                return false;
            }

            return true;
        }

        static int CheckInputStar2(string[] input)
        {
            return input.Count(ValidWordStar2);
        }

        static bool ValidWordStar2(string input)
        {
            return Star2Rule1(input) && Star2Rule2(input);
        }

        static bool Star2Rule1(string input)
        {
            // Check if combination occurs twice, but not overlapping
            bool valid = false;

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input.IndexOf(string.Concat(input[i], input[i + 1]), i + 2, StringComparison.Ordinal) != -1)
                {
                    valid = true;
                }
            }

            return valid;
        }


        static bool Star2Rule2(string input)
        {
            // Check if combination occurs twice, but not overlapping
            bool valid = false;

            for (int i = 0; i < input.Length - 2; i++)
            {
                if (input[i] == input[i + 2])
                {
                    valid = true;
                }
            }

            return valid;
        }
    }
}
