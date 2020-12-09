using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC_2020_09
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
            //Part 1
            var numbers = ParseInputIntoNumbers("./input.txt");
            if (TryFindFirstInvalidNumber(numbers, 25, out var firstInvalidNumber))
            {
                Console.WriteLine($"The first invalid number is {firstInvalidNumber}.");
            }
            else
            {
                Console.WriteLine("There is no invalid number in the input numbers list.");
                return;
            }

            //Part 2
            if (TryFindEncryptionWeakness(numbers, firstInvalidNumber, out var encryptionWeakness))
                Console.WriteLine($"The encryption weakness is {encryptionWeakness}.");
            else
                Console.WriteLine("Can't find an encryption weakness.");
        }

        private bool TryFindEncryptionWeakness(ulong[] numbers, ulong invalidNumber, out ulong encryptionWeakness)
        {
            for (var i = 0; i < numbers.Length; i++)
            {
                // var firstNumber = numbers[i];
                var total = numbers[i];
                var j = i;
                while (total < invalidNumber)
                {
                    j++;
                    total += numbers[j];

                    if (total != invalidNumber) continue;

                    var contiguousSet = numbers[i..j];


                    encryptionWeakness = contiguousSet.Min() + contiguousSet.Max();
                    return true;
                }
            }

            encryptionWeakness = 0;
            return false;
        }

        private bool TryFindFirstInvalidNumber(ulong[] numbers, int preambleSize, out ulong invalidNumber)
        {
            for (var i = preambleSize; i < numbers.Length; i++)
            {
                var testedNumber = numbers[i];
                var isValid = false;
                for (var j = 0; j < preambleSize; j++)
                {
                    var num1 = numbers[i - preambleSize + j];
                    for (var k = 0; k < preambleSize; k++)
                    {
                        if (j == k)
                            continue;

                        var num2 = numbers[i - preambleSize + k];
                        
                        if (testedNumber != num1 + num2) continue;
                        
                        isValid = true;
                        break;
                    }

                    if (isValid)
                        break;
                }

                if (isValid) continue;

                invalidNumber = testedNumber;
                return true;
            }

            invalidNumber = 0;
            return false;
        }

        private ulong[] ParseInputIntoNumbers(string inputPath)
        {
            var numbers = new List<ulong>();
            var sr = new StreamReader(inputPath);
            using (sr)
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (ulong.TryParse(line, out var number))
                        numbers.Add(number);
                    else
                        Console.WriteLine($"Couldn't parse the line {line}");
                }
            }

            return numbers.ToArray();
        }
    }
}