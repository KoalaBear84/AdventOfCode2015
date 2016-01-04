using System;
using System.IO;

namespace Day1
{
    class Program
    {
        enum Action
        {
            UP,
            DOWN,
            UNKNOWN
        }

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            int star1 = ParseInput(input);
            int star2 = ParseInput(input, -1);

            Console.WriteLine("Day 1 - Star 1, Level: {0}", star1);
            Console.WriteLine("Day 1 - Star 2, Position: {0}", star2);
            Console.ReadKey();
        }

        static int ParseInput(string input, int? stopAtLevel = null)
        {
            int level = 0;

            for (int i = 0; i < input.Length; i++)
            {
                Action action = ParseFloor(input[i]);

                level += Convert.ToInt16(action == Action.UP);
                level -= Convert.ToInt16(action == Action.DOWN);

                if (stopAtLevel.HasValue && level == stopAtLevel)
                {
                    return i + 1;
                }
            }

            return level;
        }

        static Action ParseFloor(char floorAction)
        {
            Action result = Action.UNKNOWN;

            switch (floorAction)
            {
                case '(':
                    result = Action.UP;
                    break;

                case ')':
                    result = Action.DOWN;
                    break;
            }

            return result;
        }
    }
}
