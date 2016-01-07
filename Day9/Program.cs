using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day9
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
        static Regex Regex = new Regex(@"(?<Start>\w+) to (?<Stop>\w+) = (?<Distance>\d*)");
        static List<Route> Routes = new List<Route>();

        class Route
        {
            public string Start { get; set; }
            public string Stop { get; set; }
            public int Distance { get; set; }
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            foreach (string line in input)
            {
                Routes.AddRange(ParseRoute(line));
            }

            List<Route> star1 = CalculateRoute(Routes.OrderBy(r => r.Distance).ToList());

            foreach (Route route in star1)
            {
                Console.WriteLine($"From {route.Start} to {route.Stop} = {route.Distance}");
            }

            Console.WriteLine($"Day 9 - Star 1, Answer: {star1.Sum(r => r.Distance)}");

            List<Route> star2 = CalculateRoute(Routes.OrderByDescending(r => r.Distance).ToList(), false);

            foreach (Route route in star2)
            {
                Console.WriteLine($"From {route.Start} to {route.Stop} = {route.Distance}");
            }

            Console.WriteLine($"Day 9 - Star 2, Answer: {star2.Sum(r => r.Distance)}");

            Console.ReadKey();
        }

        static List<Route> CalculateRoute(List<Route> routes, bool shortest = true)
        {
            // Get a list of all distinct cities
            List<string> cities = routes.DistinctBy(r => r.Start).Select(r => r.Start).ToList();

            // Get all possible combinations
            List<IEnumerable<string>> cityPermutations = cities.GetPermutations().ToList();

            List<List<Route>> allRoutes = new List<List<Route>>();

            // Go through all possible combinations
            foreach (IEnumerable<string> cityPermutation in cityPermutations)
            {
                List<Route> currentRoute = new List<Route>();
                string currentCity = cityPermutation.Take(1).First();

                // Go to all cities
                foreach (string nextCity in cityPermutation.Skip(1))
                {
                    currentRoute.Add(routes.First(r => r.Start == currentCity && r.Stop == nextCity));
                    currentCity = nextCity;
                }

                allRoutes.Add(currentRoute);
            }

            if (shortest)
            {
                return allRoutes.OrderBy(route => route.Sum(r => r.Distance)).First();
            } else
            {
                return allRoutes.OrderByDescending(route => route.Sum(r => r.Distance)).First();
            }
        }

        static List<Route> ParseRoute(string input)
        {
            Match regexMatch = Regex.Match(input);

            List<Route> routes = new List<Route>();

            routes.Add(new Route()
            {
                Start = regexMatch.Groups["Start"].Value,
                Stop = regexMatch.Groups["Stop"].Value,
                Distance = int.Parse(regexMatch.Groups["Distance"].Value)
            });

            routes.Add(new Route()
            {
                Start = regexMatch.Groups["Stop"].Value,
                Stop = regexMatch.Groups["Start"].Value,
                Distance = int.Parse(regexMatch.Groups["Distance"].Value)
            });

            return routes;
        }
    }
}
