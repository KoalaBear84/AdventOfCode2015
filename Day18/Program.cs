using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static bool[,] LightsArray;

        static List<KeyValuePair<int, int>> Neighbours = new List<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>( -1, -1 ),
                new KeyValuePair<int, int>( 0, -1 ),
                new KeyValuePair<int, int>( 1, -1 ),
                new KeyValuePair<int, int>( -1, 0 ),
                new KeyValuePair<int, int>( 1, 0 ),
                new KeyValuePair<int, int>( -1, 1 ),
                new KeyValuePair<int, int>( 0, 1 ),
                new KeyValuePair<int, int>( 1, 1 )
            };

        static void Main(string[] args)
        {
            ReadFramesFromDisk("input.txt");

            for (int frame = 1; frame <= 100; frame++)
            {
                Console.WriteLine($"Calculate frame {frame}");
                LightsArray = CalculateNextFrame(LightsArray);
                //WriteFrame(LightsArray, frame);
            }

            Console.WriteLine("Day 18 - Star 1, Answer: {0}", CountLights(LightsArray));

            ReadFramesFromDisk("input.txt");

            for (int frame = 1; frame <= 100; frame++)
            {
                Console.WriteLine($"Calculate frame {frame}");
                LightsArray = CalculateNextFrame(LightsArray, true);
                //WriteFrame(LightsArray, frame);
            }

            Console.WriteLine("Day 18 - Star 2, Answer: {0}", CountLights(LightsArray));
            Console.ReadKey();
        }

        static void ReadFramesFromDisk(string filename)
        {
            string[] input = File.ReadAllLines(filename);

            LightsArray = new bool[input.Length, input[0].Length];

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    LightsArray[i, j] = input[i][j] == '#';
                }
            }
        }

        static void WriteFrame(bool[,] lightsArray, int frame)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int width = lightsArray.GetLength(0);
            int height = lightsArray.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    stringBuilder.Append(lightsArray[x, y] ? "#" : ".");
                }

                stringBuilder.AppendLine();
            }

            File.WriteAllText($"Frame{frame}.txt", stringBuilder.ToString());
        }

        static int CountLights(bool[,] lights)
        {
            int lightsOn = 0;

            foreach (bool light in lights)
            {
                if (light)
                {
                    lightsOn++;
                }
            }

            return lightsOn;
        }

        static bool[,] CalculateNextFrame(bool[,] lightsArray, bool lightsStuck = false)
        {
            int width = lightsArray.GetLength(0);
            int height = lightsArray.GetLength(1);

            bool[,] lightsArrayResult = new bool[width, height];

            if (lightsStuck)
            {
                lightsArray[0, 0] = true;
                lightsArray[0, width - 1] = true;
                lightsArray[height - 1, 0] = true;
                lightsArray[height - 1, width - 1] = true;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int lightedNeighbours = GetNeighbours(lightsArray, x, y);

                    if (lightsArray[x, y])
                    {
                        lightsArrayResult[x, y] = (lightedNeighbours == 2 || lightedNeighbours == 3);
                    }
                    else
                    {
                        lightsArrayResult[x, y] = (lightedNeighbours == 3);
                    }
                }
            }

            if (lightsStuck)
            {
                lightsArrayResult[0, 0] = true;
                lightsArrayResult[0, width - 1] = true;
                lightsArrayResult[height - 1, 0] = true;
                lightsArrayResult[height - 1, width - 1] = true;
            }

            return lightsArrayResult;
        }

        static int GetNeighbours(bool[,] lightsArray, int i, int j)
        {
            int lightedNeighbours = 0;

            foreach (KeyValuePair<int, int> neighbour in Neighbours)
            {
                if (i + neighbour.Key >= 0 && i + neighbour.Key < lightsArray.GetLength(1) && j + neighbour.Value >= 0 && j + neighbour.Value < lightsArray.GetLength(1))
                {
                    lightedNeighbours += lightsArray[i + neighbour.Key, j + neighbour.Value] ? 1 : 0;
                }
            }

            return lightedNeighbours;
        }
    }
}
