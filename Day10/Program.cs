using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string star1 = "1113122113";

            for (int i = 1; i <= 40; i++)
            {
                star1 = Convert(star1);
            }

            Console.WriteLine("Day 10 - Star 1, Answer: {0}", star1.Length);

            string star2 = star1;

            for (int i = 41; i <= 50; i++)
            {
                star2 = Convert(star2);
            }

            Console.WriteLine("Day 10 - Star 2, Answer: {0}", star2.Length);

            Console.ReadKey();
        }

        static string Convert(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            List<string> parts = SplitInParts(input);

            foreach (string part in parts)
            {
                stringBuilder.Append(string.Concat(part.Length, part.First()));
            }

            return stringBuilder.ToString();
        }

        static List<string> SplitInParts(string input)
        {
            List<string> parts = new List<string>();

            string part = string.Empty;

            foreach (char currentChar in input)
            {
                if (part.Length == 0 || part.First() == currentChar)
                {
                    part += currentChar;
                }
                else
                {
                    parts.Add(part);
                    part = currentChar.ToString();
                }
            }

            parts.Add(part);

            return parts;
        }
    }
}
