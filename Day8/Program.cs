using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            Tuple<int, int> tupleStar1 = Star1(input);
            Console.WriteLine("Day 8 - Star 1, Answer: {0}", tupleStar1.Item1 - tupleStar1.Item2);

            Tuple<int, int> tupleStar2 = Star2(input);
            Console.WriteLine("Day 8 - Star 2, Answer: {0}", tupleStar2.Item2 - tupleStar2.Item1);

            Console.ReadKey();
        }

        static Tuple<int, int> Star1(string[] input)
        {
            int codeCharacters = 0;
            int memoryCharacters = 0;

            foreach (string line in input)
            {
                codeCharacters += line.Length;

                string result = Unescape(line.Substring(1, line.Length - 2));
                memoryCharacters += result.Length;
            }

            return new Tuple<int, int>(codeCharacters, memoryCharacters);
        }

        static Tuple<int, int> Star2(string[] input)
        {
            int codeCharacters = 0;
            int memoryCharacters = 0;

            foreach (string line in input)
            {
                codeCharacters += line.Length;

                string result = Escape(line);
                memoryCharacters += result.Length;
            }

            return new Tuple<int, int>(codeCharacters, memoryCharacters);
        }

        static string Unescape(string input)
        {
            return Regex.Replace(input, @"(?<BackslashEscape>\\\\)|(?<DoubleQuoteEscape>\\"")|(?<HexEscape>\\x\w{2})", delegate (Match match)
            {
                if (match.Groups["BackslashEscape"].Length > 0)
                {
                    return "\\";
                }

                if (match.Groups["DoubleQuoteEscape"].Length > 0)
                {
                    return @"""";
                }

                if (match.Groups["HexEscape"].Length > 0)
                {
                    return ((char)Convert.ToInt32(match.Groups["HexEscape"].Value.Substring(2), 16)).ToString();
                }

                return string.Empty;
            });
        }

        static string Escape(string input)
        {
            return string.Concat(@"""", Regex.Replace(input, @"(?<BackslashEscape>\\)|(?<DoubleQuoteEscape>"")", delegate (Match match)
            {
                if (match.Groups["BackslashEscape"].Length > 0)
                {
                    return "\\\\";
                }

                if (match.Groups["DoubleQuoteEscape"].Length > 0)
                {
                    return @"""""";
                }

                return string.Empty;
            }), @"""");
        }
    }
}
