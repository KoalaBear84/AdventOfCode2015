using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {
        static Regex Regex = new Regex("(?<Length>\\d+)x(?<Width>\\d+)x(?<Height>\\d+)");

        class Package
        {
            public int Length { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            List<Package> star1Packages = ParseInput(input);
            int star1 = CalculateWrapping(star1Packages);
            List<Package> star2Packages = ParseInput(input);
            int star2 = CalculateRibbon(star2Packages);

            Console.WriteLine("Day 2 - Star 1: {0}", star1);
            Console.WriteLine("Day 2 - Star 2: {0}", star2);
            Console.ReadKey();
        }

        static int CalculateWrapping(List<Package> packages)
        {
            int wrapping = 0;

            foreach (Package package in packages)
            {
                wrapping += CalculateWrapping(package);
            }

            return wrapping;
        }

        static int CalculateWrapping(Package package)
        {
            int side1 = package.Length * package.Width;
            int side2 = package.Width * package.Height;
            int side3 = package.Height * package.Length;

            int wrapping = (2 * side1) + (2 * side2) + (2 * side3) + (new[] { side1, side2, side3 }.Min());

            return wrapping;
        }

        static int CalculateRibbon(List<Package> packages)
        {
            int ribbon = 0;

            foreach (Package package in packages)
            {
                ribbon += CalculateRibbon(package);
            }

            return ribbon;
        }

        static int CalculateRibbon(Package package)
        {
            IEnumerable<int> smallest = new[] { package.Length, package.Width, package.Height }.OrderBy(x => x).Take(2);

            int ribbon = (smallest.First() * 2) + (smallest.Last() * 2) + (package.Length * package.Width * package.Height);

            return ribbon;
        }

        static List<Package> ParseInput(string[] input)
        {
            List<Package> packages = new List<Package>();

            foreach (string line in input)
            {
                packages.Add(ParseLine(line));
            }

            return packages;
        }

        static Package ParseLine(string line)
        {
            Package package = new Package();

            Match regexMatch = Regex.Match(line);

            if (regexMatch.Success)
            {
                package.Length = int.Parse(regexMatch.Groups["Length"].Value);
                package.Width = int.Parse(regexMatch.Groups["Width"].Value);
                package.Height = int.Parse(regexMatch.Groups["Height"].Value);
            }

            return package;
        }
    }
}
