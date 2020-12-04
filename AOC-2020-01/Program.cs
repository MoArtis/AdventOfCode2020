using System;
using System.IO;
using System.Linq;

namespace AOC_2020_01
{
    class Program
    {
        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            const string inputPath = "./input.txt";

            if (File.Exists(inputPath) == false)
                return;
            
            var numbers = File.ReadAllLines(inputPath).Select(int.Parse).ToArray();
            Array.Sort(numbers);

            SearchForTwoNumbersAddingTo(numbers, 2020);
            SearchForThreeNumbersAddingTo(numbers, 2020);
        }

        private void SearchForTwoNumbersAddingTo(int[] sortedNumbers, int totalNumber)
        {
            for (int i = 0; i < sortedNumbers.Length; i++)
            {
                var foundIndex = SearchForNumberToTotal(sortedNumbers, sortedNumbers[i], totalNumber);
                
                if (foundIndex < 0 || foundIndex == i) continue;

                Console.WriteLine(
                    $"The answer is {sortedNumbers[i]} * {sortedNumbers[foundIndex]} = {sortedNumbers[i] * sortedNumbers[foundIndex]}");
                return;
            }
        }
        
        private void SearchForThreeNumbersAddingTo(int[] sortedNumbers, int totalNumber)
        {
            for (var i = 0; i < sortedNumbers.Length; i++)
            {
                for (var j = 0; j < sortedNumbers.Length; j++)
                {
                    if (i == j)
                        continue;

                    var firstAdd = sortedNumbers[i] + sortedNumbers[j];
                    var foundIndex = SearchForNumberToTotal(sortedNumbers, firstAdd, totalNumber);

                    if (foundIndex < 0 || foundIndex == i || foundIndex == j) continue;

                    Console.WriteLine(
                        $"The answer is {sortedNumbers[i]} * {sortedNumbers[j]} * {sortedNumbers[foundIndex]} = {sortedNumbers[i] * sortedNumbers[j] * sortedNumbers[foundIndex]}");
                    return;
                }
            }

            Console.WriteLine($"There is no answer to the question");
        }

        private static int SearchForNumberToTotal(int[] numbers, int startNumber, int total)
        {
            var searchedNumber = total - startNumber;
            var pickedIndex = Array.BinarySearch(numbers, searchedNumber);

            //~pickedIndex for the closest index
            return pickedIndex >= 0 ? pickedIndex : -1;
        }
    }
}