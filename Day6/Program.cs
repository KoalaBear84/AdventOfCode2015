using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day6
{
    class Program
    {
        static Regex Regex = new Regex(@"^(?<Action>.*) (?<StartX>\d*),(?<StartY>\d*) through (?<EndX>\d*),(?<EndY>\d*)$");

        enum Action
        {
            TURNON,
            TURNOFF,
            TOGGLE,
            UNKNOWN
        }

        static int[,] LightsArray = new int[1000, 1000];

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            Console.WriteLine("Day 6 - Star 1, Answer: {0}", ProcessInput(input));

            LightsArray = new int[1000, 1000];
            Console.WriteLine("Day 6 - Star 2, Answer: {0}", ProcessInput(input, false));
            Console.ReadKey();
        }

        static int CountLights(int[,] lights)
        {
            int lightsOn = 0;

            foreach (int light in lights)
            {
                lightsOn += light;
            }

            return lightsOn;
        }

        static int ProcessInput(string[] input, bool star1 = true)
        {
            foreach (string line in input)
            {
                ParseLine(line, star1);
            }

            return CountLights(LightsArray);
        }

        static void ParseLine(string input, bool star1 = true)
        {
            Match regexMatch = Regex.Match(input);

            Action action = ParseAction(regexMatch.Groups["Action"].Value);
            int startx = int.Parse(regexMatch.Groups["StartX"].Value);
            int starty = int.Parse(regexMatch.Groups["StartY"].Value);
            int endx = int.Parse(regexMatch.Groups["EndX"].Value);
            int endy = int.Parse(regexMatch.Groups["EndY"].Value);

            for (int x = startx; x <= endx; x++)
            {
                for (int y = starty; y <= endy; y++)
                {
                    switch (action)
                    {
                        case Action.TURNON:
                            if (star1)
                            {
                                LightsArray[x, y] = 1;
                            }
                            else
                            {
                                LightsArray[x, y]++;
                            }
                            break;

                        case Action.TURNOFF:
                            LightsArray[x, y] = Math.Max(0, --LightsArray[x, y]);
                            break;

                        case Action.TOGGLE:
                            if (star1)
                            {
                                LightsArray[x, y] = (LightsArray[x, y] == 0 ? 1 : 0);
                            }
                            else
                            {
                                LightsArray[x, y] += 2;
                            }
                            break;
                    }
                }
            }
        }

        static Action ParseAction(string input)
        {
            Action action = Action.UNKNOWN;

            switch (input)
            {
                case "turn on":
                    action = Action.TURNON;
                    break;

                case "turn off":
                    action = Action.TURNOFF;
                    break;

                case "toggle":
                    action = Action.TOGGLE;
                    break;
            }

            return action;
        }
    }
}
