using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day13
{
    static class ExtensionMethods2
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }

    class Program
    {
        static Regex Regex = new Regex(@"(?<Person1>\w+) would (?<LoseGain>\w+) (?<Happiness>\d+) happiness units by sitting next to (?<Person2>\w+).");
        static List<Connection> Connections = new List<Connection>();

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            foreach (string line in input)
            {
                Connections.AddRange(ParseConnection(line));
            }

            List<Connection> star1 = CalculateBestPlacement(Connections.OrderBy(r => r.Happiness).ToList());

            foreach (Connection connection in star1)
            {
                Console.WriteLine($"From {connection.Person1} to {connection.Person2} = {connection.Happiness}");
            }

            Console.WriteLine($"Day 13 - Star 1, Answer: {star1.Sum(c => c.Happiness)}");

            List<string> people = Connections.DistinctBy(r => r.Person1).Select(r => r.Person1).ToList();

            // Add yourself
            foreach (string person in people)
            {
                Connections.Add(new Connection
                {
                    Person1 = "Bas",
                    Person2 = person
                });

                Connections.Add(new Connection
                {
                    Person1 = person,
                    Person2 = "Bas"
                });
            }

            List<Connection> star2 = CalculateBestPlacement(Connections.OrderByDescending(r => r.Happiness).ToList());

            foreach (Connection connection in star2)
            {
                Console.WriteLine($"From {connection.Person1} to {connection.Person2} = {connection.Happiness}");
            }

            Console.WriteLine($"Day 13 - Star 2, Answer: {star2.Sum(c => c.Happiness)}");

            Console.ReadKey();
        }

        static List<Connection> CalculateBestPlacement(List<Connection> peopleConnections, bool unhappy = false)
        {
            // Get a list of all distinct people
            List<string> people = peopleConnections.DistinctBy(r => r.Person1).Select(r => r.Person1).ToList();

            // Get all possible combinations
            List<IEnumerable<string>> peoplePermutations = people.GetPermutations().ToList();

            List<List<Connection>> allPlacements = new List<List<Connection>>();

            // Go through all possible combinations
            foreach (IEnumerable<string> personPermutation in peoplePermutations)
            {
                List<Connection> currenPlacement = new List<Connection>();
                string currentPerson = personPermutation.First();

                // Go to all people
                foreach (string nextPerson in personPermutation.Skip(1))
                {
                    currenPlacement.Add(peopleConnections.First(r => r.Person1 == currentPerson && r.Person2 == nextPerson));
                    currenPlacement.Add(peopleConnections.First(r => r.Person1 == nextPerson && r.Person2 == currentPerson));

                    currentPerson = nextPerson;
                }

                // Also connect the first and last person
                currenPlacement.Add(peopleConnections.First(r => r.Person1 == personPermutation.Last() && r.Person2 == personPermutation.First()));
                currenPlacement.Add(peopleConnections.First(r => r.Person1 == personPermutation.First() && r.Person2 == personPermutation.Last()));

                allPlacements.Add(currenPlacement);
            }

            if (unhappy)
            {
                return allPlacements.OrderBy(route => route.Sum(r => r.Happiness)).First();
            }
            else
            {
                return allPlacements.OrderByDescending(route => route.Sum(r => r.Happiness)).First();
            }
        }

        static List<Connection> ParseConnection(string input)
        {
            Match regexMatch = Regex.Match(input);

            List<Connection> connections = new List<Connection>();

            connections.Add(new Connection()
            {
                Person1 = regexMatch.Groups["Person1"].Value,
                Person2 = regexMatch.Groups["Person2"].Value,
                Happiness = regexMatch.Groups["LoseGain"].Value == "gain" ? int.Parse(regexMatch.Groups["Happiness"].Value) : int.Parse(regexMatch.Groups["Happiness"].Value) * -1
            });

            return connections;
        }
    }

    class Connection
    {
        public string Person1 { get; set; }
        public string Person2 { get; set; }
        public int Happiness { get; set; }
    }
}
