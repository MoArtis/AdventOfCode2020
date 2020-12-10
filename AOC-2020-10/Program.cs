using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC_2020_10
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            var inputPath = "./input.txt";
            // var inputPath = "./testInput1.txt";
            // var inputPath = "./testInput2.txt";

            var adapters = GenerateAdaptersList(inputPath);

            //Part 1
            var oneJoltDiffCount = CountJoltageDifferences(adapters, 1);
            var threeJoltDiffCount = CountJoltageDifferences(adapters, 3);
            Console.WriteLine(
                $"The part 1 answer is : {oneJoltDiffCount} * {threeJoltDiffCount} = {oneJoltDiffCount * threeJoltDiffCount}");

            //Part 2            
            var permutation = CountPermutations(adapters);
            Console.WriteLine($"The number of permutation is : {permutation}");
        }

        private int CountJoltageDifferences(int[] adapters, int joltDifference)
        {
            var total = 0;
            for (var i = 1; i < adapters.Length; i++)
            {
                if (adapters[i] - adapters[i - 1] == joltDifference)
                    total++;
            }

            return total;
        }

        private int[] GenerateAdaptersList(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return null;

            var adapterStrings = File.ReadAllLines(inputPath);
            var adapters = new int[adapterStrings.Length + 2];
            for (var i = 0; i < adapterStrings.Length; i++)
            {
                adapters[i + 1] = int.Parse(adapterStrings[i]);
            }

            Array.Sort(adapters, 0, adapters.Length - 1);

            adapters[^1] = adapters[^2] + 3;

            return adapters;
        }

        private ulong CountPermutations(int[] adapters)
        {
            var countTracker = new Dictionary<int, ulong> {[adapters.Length - 1] = 1};

            for (var i = adapters.Length - 2; i >= 0; i--)
            {
                ulong currentCount = 0;
                for (var j = i + 1; j < adapters.Length; j++)
                {
                    if (adapters[j] - adapters[i] > 3)
                        break;
                    
                    currentCount += countTracker[j];
                }

                countTracker[i] = currentCount;
            }

            return countTracker[0];
        }
    }
}