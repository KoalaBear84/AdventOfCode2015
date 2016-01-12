using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        static readonly Regex Regex = new Regex(@"Sue (?<SueNumber>\d+): (?<Objects>.*)");
        static List<Sue> Sues = new List<Sue>();

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            foreach (string line in input)
            {
                Sues.Add(ParseSue(line));
            }

            List<Property> commonProperties = new List<Property>
            {
                new Property { Name = "children", Amount = 3 },
                new Property { Name = "cats", Amount = 7 },
                new Property { Name = "samoyeds", Amount = 2 },
                new Property { Name = "pomeranians", Amount = 3 },
                new Property { Name = "akitas", Amount = 0 },
                new Property { Name = "vizslas", Amount = 0 },
                new Property { Name = "goldfish", Amount = 5 },
                new Property { Name = "trees", Amount = 3 },
                new Property { Name = "cars", Amount = 2 },
                new Property { Name = "perfumes", Amount = 1 }
            };

            foreach (Sue sue in Sues)
            {
                foreach (Property commonProperty in commonProperties)
                {
                    sue.Properties.RemoveAll(p => p.Name == commonProperty.Name && p.Amount != commonProperty.Amount);
                }
            }

            Sue bestMatchingSue = Sues.OrderByDescending(s => s.Properties.Count).FirstOrDefault();

            Console.WriteLine($"Day 16 - Star 1, Answer: {bestMatchingSue.Number}");

            // Get back original Sues
            Sues.Clear();

            foreach (string line in input)
            {
                Sues.Add(ParseSue(line));
            }

            foreach (Sue sue in Sues)
            {
                foreach (Property commonProperty in commonProperties)
                {
                    if (commonProperty.Name == "cats" || commonProperty.Name == "trees")
                    {
                        sue.Properties.RemoveAll(p => p.Name == commonProperty.Name && p.Amount <= commonProperty.Amount);
                    } else
                    {
                        if (commonProperty.Name == "pomeranians" || commonProperty.Name == "goldfish")
                        {
                            sue.Properties.RemoveAll(p => p.Name == commonProperty.Name && p.Amount >= commonProperty.Amount);
                        }
                        else
                        {
                            sue.Properties.RemoveAll(p => p.Name == commonProperty.Name && p.Amount != commonProperty.Amount);
                        }
                    }
                }
            }

            bestMatchingSue = Sues.OrderByDescending(s => s.Properties.Count).FirstOrDefault();

            Console.WriteLine($"Day 16 - Star 2, Answer: {bestMatchingSue.Number}");

            Console.ReadKey();
        }

        static Sue ParseSue(string input)
        {
            Match regexMatch = Regex.Match(input);

            string propertiesString = regexMatch.Groups["Objects"].Value;
            IEnumerable<string> properties = propertiesString.Split(',');

            return new Sue
            {
                Number = int.Parse(regexMatch.Groups["SueNumber"].Value),
                Properties = properties.Select(p => new Property
                {
                    Name = p.Split(':').First().Trim(),
                    Amount = int.Parse(p.Split(':').Last())
                }).ToList()
            };
        }
    }

    class Sue
    {
        public int Number { get; set; }
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
