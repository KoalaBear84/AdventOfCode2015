using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        enum Action
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            UNKNOWN
        }

        public class Present
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            List<Present> star1Presents = ParseInput(input);
            int star1UniqueLocations = star1Presents.Select(p => new { p.X, p.Y }).Distinct().Count();
            List<Present> star2Presents = ParseInput(input, true);
            int star2UniqueLocations = star2Presents.Select(p => new { p.X, p.Y }).Distinct().Count();

            Console.WriteLine("Day 3 - Star 1, Houses visited: {0}", star1UniqueLocations);
            Console.WriteLine("Day 3 - Star 2, Houses visited: {0}", star2UniqueLocations);
            Console.ReadKey();
        }

        static List<Present> ParseInput(string input, bool roboSanta = false)
        {
            List<Present> presents = new List<Present>();

            int santaX = 0;
            int santaY = 0;
            int roboSantaX = 0;
            int roboSantaY = 0;

            // Starting point present
            presents.Add(new Present { X = santaX, Y = santaY });

            if (roboSanta)
            {
                presents.Add(new Present { X = roboSantaX, Y = roboSantaY });
            }

            bool santasTurn = true;

            // Other presents
            for (int i = 0; i < input.Length; i++)
            {
                Action direction = ParseDirection(input[i]);

                switch (direction)
                {
                    case Action.UP:
                        if (santasTurn) { santaY--; } else { roboSantaY--; }
                        break;

                    case Action.RIGHT:
                        if (santasTurn) { santaX++; } else { roboSantaX++; }
                        break;

                    case Action.DOWN:
                        if (santasTurn) { santaY++; } else { roboSantaY++; }
                        break;

                    case Action.LEFT:
                        if (santasTurn) { santaX--; } else { roboSantaX--; }
                        break;
                }

                if (santasTurn)
                {
                    presents.Add(new Present { X = santaX, Y = santaY });
                } else
                {
                    presents.Add(new Present { X = roboSantaX, Y = roboSantaY });
                }

                if (roboSanta)
                {
                    santasTurn = !santasTurn;
                }
            }

            return presents;
        }

        static Action ParseDirection(char direction)
        {
            Action result = Action.UNKNOWN;

            switch (direction)
            {
                case '<':
                    result = Action.LEFT;
                    break;

                case '^':
                    result = Action.UP;
                    break;

                case 'v':
                    result = Action.DOWN;
                    break;

                case '>':
                    result = Action.RIGHT;
                    break;
            }

            return result;
        }
    }
}
