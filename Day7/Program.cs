using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day7
{
    class Program
    {
        static Regex Regex = new Regex(@"^((?<Input1>\w+) -> (?<Output>\w+))|((?<Input1>\w+) (?<Action>AND|OR) (?<Input2>\w+) -> (?<Output>\w+))|((?<Input1>\w+) (?<Action>LSHIFT|RSHIFT) (?<Parameter>\w+) -> (?<Output>\w+))|((?<Action>NOT) (?<Input1>\w+) -> (?<Output>\w+))$");

        static Dictionary<string, ushort> Outputs = new Dictionary<string, ushort>();
        static List<Rule> Rules = new List<Rule>();

        class Rule
        {
            public bool Processed { get; set; }
            public string Raw { get; set; }
            public Action Action { get; set; }
            public string Input1 { get; set; }
            public string Input2 { get; set; }
            public string Output { get; set; }
            public string Parameter { get; set; }
        }

        enum Action
        {
            SIGNAL,
            AND,
            OR,
            LSHIFT,
            RSHIFT,
            NOT,
            UNKNOWN
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            ParseRules(input);

            int iterations = 0;

            while (Rules.Any(r => !r.Processed))
            {
                ProcessRules();
                iterations++;
            }

            foreach (KeyValuePair<string, ushort> output in Outputs.OrderBy(op => op.Key))
            {
                Console.WriteLine($"{output.Key}: {output.Value}");
            }

            Console.WriteLine($"Day 7 - Star 1, Answer: {Outputs["a"]}");

            // Signal on a to b
            Outputs["b"] = Outputs["a"];

            // Reset all signals
            foreach(KeyValuePair<string, ushort> output in Outputs.Where(op => op.Key != "b").ToList())
            {
                Outputs.Remove(output.Key);
            }

            // Reset all rules
            foreach(Rule rule in Rules)
            {
                // Do not overwrite b
                if (rule.Output != "b")
                {
                    rule.Processed = false;
                }
            }

            // Process all rules again
            while (Rules.Any(r => !r.Processed))
            {
                ProcessRules();
                iterations++;
            }

            Console.WriteLine("Day 7 - Star 2, Answer: {0}", Outputs["a"]);
            Console.ReadKey();
        }

        static void ProcessRules()
        {
            foreach (Rule rule in Rules.Where(r => !r.Processed))
            {
                ProcessRule(rule);
            }
        }

        static bool GetInput(string inputString, out ushort input)
        {
            if (inputString.All(char.IsDigit))
            {
                input = ushort.Parse(inputString);
                return true;
            }

            if (Outputs.ContainsKey(inputString))
            {
                input = Outputs[inputString];
                return true;
            }

            input = 0;
            return false;
        }

        static bool ProcessRule(Rule rule)
        {
            if (rule.Processed) return false;

            ushort input1 = 0;
            ushort input2 = 0;

            switch (rule.Action)
            {
                case Action.SIGNAL:
                    if (GetInput(rule.Input1, out input1))
                    {
                        Outputs[rule.Output] = input1;
                        rule.Processed = true;
                    }
                    break;

                case Action.AND:
                    if (GetInput(rule.Input1, out input1) && GetInput(rule.Input2, out input2))
                    {
                        Outputs[rule.Output] = (ushort)(input1 & input2);
                        rule.Processed = true;
                    }
                    break;

                case Action.OR:
                    if (GetInput(rule.Input1, out input1) && GetInput(rule.Input2, out input2))
                    {
                        Outputs[rule.Output] = (ushort)(input1 | input2);
                        rule.Processed = true;
                    }
                    break;

                case Action.NOT:
                    if (GetInput(rule.Input1, out input1))
                    {
                        Outputs[rule.Output] = (ushort)(~input1);
                        rule.Processed = true;
                    }
                    break;

                case Action.LSHIFT:
                    if (GetInput(rule.Input1, out input1))
                    {
                        Outputs[rule.Output] = (ushort)(input1 << ushort.Parse(rule.Parameter));
                        rule.Processed = true;
                    }
                    break;

                case Action.RSHIFT:
                    if (GetInput(rule.Input1, out input1))
                    {
                        Outputs[rule.Output] = (ushort)(input1 >> ushort.Parse(rule.Parameter));
                        rule.Processed = true;
                    }
                    break;
            }

            return rule.Processed;
        }

        static void ParseRules(string[] input)
        {
            foreach (string line in input)
            {
                Rules.Add(ParseRule(line));
            }
        }

        static Rule ParseRule(string input)
        {
            Match regexMatch = Regex.Match(input);

            return new Rule()
            {
                Raw = input,
                Action = ParseAction(regexMatch.Groups["Action"].Value),
                Input1 = regexMatch.Groups["Input1"].Value,
                Input2 = regexMatch.Groups["Input2"].Value,
                Output = regexMatch.Groups["Output"].Value,
                Parameter = regexMatch.Groups["Parameter"].Value
            };
        }

        static Action ParseAction(string input)
        {
            Action action = Action.UNKNOWN;

            switch (input)
            {
                case "AND":
                    action = Action.AND;
                    break;

                case "OR":
                    action = Action.OR;
                    break;

                case "LSHIFT":
                    action = Action.LSHIFT;
                    break;

                case "RSHIFT":
                    action = Action.RSHIFT;
                    break;

                case "NOT":
                    action = Action.NOT;
                    break;

                default:
                    action = Action.SIGNAL;
                    break;
            }

            return action;
        }

    }
}
