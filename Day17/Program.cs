using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static int[] Containers = { 43, 3, 4, 10, 21, 44, 4, 6, 47, 41, 34, 17, 17, 44, 36, 31, 46, 9, 27, 38 };

        static void Main(string[] args)
        {
            IEnumerable<List<int>> combinations = PowerSet(Containers.ToList()).Where(x => x.Sum() == 150);

            Console.WriteLine($"Day 17 - Star 1, Answer: {combinations.Count()}");

            int minimumContainers = combinations.Min(x => x.Count);

            Console.WriteLine($"Day 17 - Star 2, Answer: {combinations.Where(x => x.Count == minimumContainers).Count()}");

            Console.ReadKey();
        }

        static List<List<T>> PowerSet<T>(List<T> list)
        {
            List<List<T>> powerset = new List<List<T>> { new List<T>() };

            foreach (T item in list)
            {
                foreach (List<T> set in powerset.ToArray())
                {
                    powerset.Add(new List<T>(set) { item });
                }
            }

            return powerset;
        }
    }
}
