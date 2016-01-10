using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Day12
{
    class Program
    {
        static int total = 0;

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            JObject numbers = JsonConvert.DeserializeObject<JObject>(input);

            WalkNode(numbers, n => { });

            int star1 = total;

            Console.WriteLine($"Day 12 - Star 1, Answer: {star1}");

            total = 0;

            WalkNode(numbers, n => { }, true);

            int star2 = total;

            Console.WriteLine($"Day 12 - Star 2, Answer: {star2}");

            Console.ReadKey();
        }

        static bool WalkNode(JToken node, Action<JObject> action, bool skipRed = false)
        {
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                int totalBeforeRed = total;

                foreach (JProperty child in node.Children<JProperty>())
                {
                    if (!WalkNode(child.Value, action, skipRed))
                    {
                        if (skipRed)
                        {
                            total = totalBeforeRed;
                            break;
                        }
                    }
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                int totalBeforeRed = total;

                foreach (JToken child in node.Children())
                {
                    if (!WalkNode(child, action, skipRed))
                    {
                        if (skipRed)
                        {
                            total = totalBeforeRed;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (node.Type == JTokenType.String)
                {
                    if (node.Parent.Type != JTokenType.Array)
                    {
                        if (node.Value<string>() == "red")
                        {
                            return false;
                        }
                    }
                }

                if (node.Type == JTokenType.Integer)
                {
                    total += node.Value<int>();
                }
            }

            return true;
        }
    }
}
