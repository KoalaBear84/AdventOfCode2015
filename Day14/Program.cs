using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        static readonly Regex Regex = new Regex(@"(?<Name>\w+) can fly (?<Speed>\d+) km/s for (?<Endurance>\d+) seconds, but then must rest for (?<Rest>\d+) seconds.");
        static List<Reindeer> Reindeers = new List<Reindeer>();

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            foreach (string line in input)
            {
                Reindeers.Add(ParseReindeer(line));
            }

            Dictionary<Reindeer, int> raceDistancesStar1 = new Dictionary<Reindeer, int>();
            raceDistancesStar1 = CalculateRace(2503);

            foreach (KeyValuePair<Reindeer, int> raceDistance in raceDistancesStar1.OrderByDescending(d => d.Value))
            {
                Console.WriteLine($"Reindeer: {raceDistance.Key.Name}, Distance: {raceDistance.Value}");
            }

            Console.WriteLine($"Day 14 - Star 1, Answer: {raceDistancesStar1.OrderByDescending(rd => rd.Value).First().Key.Name} - {raceDistancesStar1.OrderByDescending(rd => rd.Value).First().Value} km");

            Dictionary<Reindeer, int> extraPoints = new Dictionary<Reindeer, int>();

            Dictionary<Reindeer, int> raceDistancesStar2 = new Dictionary<Reindeer, int>();

            for (int i = 1; i <= 2503; i++)
            {
                raceDistancesStar2 = CalculateRace(i);

                // Give current leading reindeer(s) bonus point
                IEnumerable<KeyValuePair<Reindeer, int>> leadingReindeers = raceDistancesStar2.Where(rd => rd.Value == raceDistancesStar2.Max(d => d.Value));

                foreach (KeyValuePair<Reindeer, int> leadingReindeer in leadingReindeers)
                {
                    if (!extraPoints.ContainsKey(leadingReindeer.Key))
                    {
                        extraPoints.Add(leadingReindeer.Key, 0);
                    }

                    extraPoints[leadingReindeer.Key]++;
                }
            }

            foreach (KeyValuePair<Reindeer, int> raceDistance in raceDistancesStar2.OrderByDescending(d => d.Value))
            {
                int points = 0;
                extraPoints.TryGetValue(raceDistance.Key, out points);

                Console.WriteLine($"Reindeer: {raceDistance.Key.Name}, Distance: {raceDistance.Value}, Points: {points}");
            }

            Console.WriteLine($"Day 14 - Star 2, Answer: {extraPoints.OrderByDescending(rd => rd.Value).First().Key.Name} - {extraPoints.OrderByDescending(rd => rd.Value).First().Value} points");

            Console.ReadKey();
        }

        static Dictionary<Reindeer, int> CalculateRace(int seconds)
        {
            Dictionary<Reindeer, int> distances = new Dictionary<Reindeer, int>();

            foreach (Reindeer reindeer in Reindeers)
            {
                int fullCycle = reindeer.Endurance + reindeer.Rest;
                int cycles = seconds / fullCycle;
                int distance =
                    (cycles * reindeer.Endurance * reindeer.Speed) +
                    (Math.Min(seconds - cycles * fullCycle, reindeer.Endurance) * reindeer.Speed);

                distances.Add(reindeer, distance);
            }

            return distances;
        }

        static Reindeer ParseReindeer(string input)
        {
            Match regexMatch = Regex.Match(input);

            return new Reindeer
            {
                Name = regexMatch.Groups["Name"].Value,
                Speed = int.Parse(regexMatch.Groups["Speed"].Value),
                Endurance = int.Parse(regexMatch.Groups["Endurance"].Value),
                Rest = int.Parse(regexMatch.Groups["Rest"].Value)
            };
        }
    }

    class Reindeer
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Endurance { get; set; }
        public int Rest { get; set; }
    }
}
