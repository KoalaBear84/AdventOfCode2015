using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day15
{
    class Program
    {
        static readonly Regex Regex = new Regex(@"(?<Name>\w+): capacity (?<Capacity>-?\d+), durability (?<Durability>-?\d+), flavor (?<Flavor>-?\d+), texture (?<Texture>-?\d+), calories (?<Calories>-?\d+)");
        static List<Ingredient> Ingredients = new List<Ingredient>();

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            foreach (string line in input)
            {
                Ingredients.Add(ParseIngredient(line));
            }

            List<Dictionary<Ingredient, int>> ingredientCombinations = new List<Dictionary<Ingredient, int>>();

            // Loop through all possible combinaties of ingredients
            for (int ingredient1 = 1; ingredient1 < 97; ingredient1++)
            {
                for (int ingredient2 = 1; ingredient2 < (100 - ingredient1); ingredient2++)
                {
                    for (int ingredient3 = 1; ingredient3 < (100 - ingredient1 - ingredient2); ingredient3++)
                    {
                        Dictionary<Ingredient, int> list = new Dictionary<Ingredient, int>();

                        list.Add(Ingredients[0], ingredient1);
                        list.Add(Ingredients[1], ingredient2);
                        list.Add(Ingredients[2], ingredient3);
                        list.Add(Ingredients[3], 100 - ingredient1 - ingredient2 - ingredient3);

                        ingredientCombinations.Add(list);
                    }
                }
            }

            Dictionary<Ingredient, int> bestCombination = ingredientCombinations.OrderByDescending(s => CalculateScore(s)).First();

            Console.WriteLine($"Day 15 - Star 1, Answer: {CalculateScore(bestCombination)}");

            Dictionary<Ingredient, int> bestCombination500Calories = ingredientCombinations.Where(ic => ic.Sum(ic2 => ic2.Key.Calories * ic2.Value) == 500).OrderByDescending(s => CalculateScore(s)).First();

            Console.WriteLine($"Day 15 - Star 2, Answer: {CalculateScore(bestCombination500Calories)}");

            Console.ReadKey();
        }

        static int CalculateScore(Dictionary<Ingredient, int> ingredients)
        {
            int totalCapacity = ingredients.Sum(ig => ig.Value * ig.Key.Capacity);
            int totalDurability = ingredients.Sum(ig => ig.Value * ig.Key.Durability);
            int totalFlavor = ingredients.Sum(ig => ig.Value * ig.Key.Flavor);
            int totalTexture = ingredients.Sum(ig => ig.Value * ig.Key.Texture);

            if (totalCapacity < 0 || totalDurability < 0 || totalFlavor < 0 || totalTexture < 0)
            {
                return 0;
            }

            return totalCapacity * totalDurability * totalFlavor * totalTexture;
        }

        static Ingredient ParseIngredient(string input)
        {
            Match regexMatch = Regex.Match(input);

            return new Ingredient
            {
                Name = regexMatch.Groups["Name"].Value,
                Capacity = int.Parse(regexMatch.Groups["Capacity"].Value),
                Durability = int.Parse(regexMatch.Groups["Durability"].Value),
                Flavor = int.Parse(regexMatch.Groups["Flavor"].Value),
                Texture = int.Parse(regexMatch.Groups["Texture"].Value),
                Calories = int.Parse(regexMatch.Groups["Calories"].Value)
            };
        }
    }

    class Ingredient
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavor { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }
    }
}
