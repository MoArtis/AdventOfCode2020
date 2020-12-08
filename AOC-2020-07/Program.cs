using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC_2020_07
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "./input.txt";
            // const string inputPath = "./testInput.txt";
            // const string inputPath = "./testInputPart2.txt";

            var bagRulesProcessor = new BagRulesProcessor();
            bagRulesProcessor.ProcessInput(inputPath);
        }

        internal class BagRulesProcessor
        {
            public class Bag
            {
                public Bag(string color)
                {
                    Color = color;
                }

                public string Color { get; private set; }

                public Dictionary<Bag, int> content = new Dictionary<Bag, int>();
                public List<Bag> Containers = new List<Bag>();
            }

            private Dictionary<string, Bag> _bags = new Dictionary<string, Bag>();

            public void ProcessInput(string inputPath)
            {
                GenerateBagsGraph(inputPath);

                var bagColor = "shiny gold";

                //Part 1
                var possibleContainerCount = CountPossibleContainers(bagColor);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"The bag color \"{bagColor}\" can be contained by that many other bag colors: {possibleContainerCount}");
                Console.ForegroundColor = ConsoleColor.White;

                //Part 2
                var contentCount = CountContainedBags(bagColor);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"The bag color \"{bagColor}\" contains that many bags: {contentCount}");
                Console.ForegroundColor = ConsoleColor.White;

                //Part 2 - Iterative
                contentCount = CountContainedBagsIterative(bagColor);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"(Iterative version) The bag color \"{bagColor}\" contains that many bags: {contentCount}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            private void GenerateBagsGraph(string inputPath)
            {
                if (File.Exists(inputPath) == false)
                    return;

                var sr = new StreamReader(inputPath);
                using (sr)
                {
                    var rulesSb = new StringBuilder();

                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        var splitLine = line.Split(" bags contain ");

                        var containerColor = splitLine[0];

                        if (splitLine[1].Contains("no"))
                            continue;

                        var contentRules = splitLine[1].Split(", ");

                        Console.WriteLine(containerColor);
                        Console.ForegroundColor = ConsoleColor.Green;
                        for (int i = 0; i < contentRules.Length; i++)
                        {
                            var contentRuleSplit = contentRules[i].Split(" ");
                            if (!int.TryParse(contentRuleSplit[0], out var contentBagQty))
                            {
                                Console.WriteLine($"Couldn't parse the bag qty on rule: {contentRules[i]}");
                                continue;
                            }

                            var contentBagColor = $"{contentRuleSplit[1]} {contentRuleSplit[2]}";

                            AddBagContent(containerColor, contentBagColor, contentBagQty);

                            Console.WriteLine($"{contentRules[i]} => {contentBagQty} | {contentBagColor}");
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            private void AddBagContent(string containerColor, string contentColor, int contentQty)
            {
                AddBag(containerColor);
                AddBag(contentColor);

                var containerBag = _bags[containerColor];
                var contentBag = _bags[contentColor];

                containerBag.content.TryAdd(contentBag, contentQty);
                contentBag.Containers.Add(containerBag);
            }

            private void AddBag(string color)
            {
                _bags.TryAdd(color, new Bag(color));

                // if (!_bags.ContainsKey(color))
                //     _bags.Add(color, new Bag(color));
            }

            private int CountPossibleContainers(string contentColor)
            {
                if (_bags.ContainsKey(contentColor) == false)
                    return 0;

                var contentBag = _bags[contentColor];

                var bagQueue = new Queue<Bag>(contentBag.Containers);
                var possibleContainers = new List<Bag>(contentBag.Containers);
                while (bagQueue.Count > 0)
                {
                    var containerBag = bagQueue.Dequeue();
                    foreach (var bag in containerBag.Containers)
                    {
                        possibleContainers.Add(bag);
                        bagQueue.Enqueue(bag);
                    }
                }

                return possibleContainers.Distinct().Count();
            }

            private int CountContainedBags(string containerColor)
            {
                if (_bags.ContainsKey(containerColor) == false)
                    return 0;

                var bag = _bags[containerColor];

                var bagQty = 0;
                foreach (var contentBag in bag.content.Keys)
                {
                    bagQty += bag.content[contentBag] * (CountContainedBags(contentBag.Color) + 1);
                }

                return bagQty;

                // LINQ version
                // return bag.content.Keys.Sum(contentBag => bag.content[contentBag] * (CountContainedBags(contentBag.Color) + 1));
            }

            private int CountContainedBagsIterative(string containerColor)
            {
                if (_bags.ContainsKey(containerColor) == false)
                    return 0;

                var stack = new Stack<Tuple<int, Bag>>();
                stack.Push(new Tuple<int, Bag>(1, _bags[containerColor]));

                var total = 0;
                while (stack.Count > 0)
                {
                    var (qty, bag) = stack.Pop();
                    total += qty;
                    foreach (var contentBag in bag.content.Keys)
                    {
                        stack.Push(new Tuple<int, Bag>(qty * bag.content[contentBag], contentBag));
                    }
                }

                return total - 1;
            }
        }
    }
}