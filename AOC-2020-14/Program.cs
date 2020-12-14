using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC_2020_14
{
    internal class Program
    {
        private static void Main()
        {
            var program = new Program();
            program.Run();
        }

        private readonly Dictionary<long, ulong> _mem = new Dictionary<long, ulong>();
        private ulong _maskZeros = 0;
        private ulong _maskOnes = 0;
        private string _mask = "";

        private void Run()
        {
            var inputPath = "./input.txt";
            // var inputPath = "./testInput.txt";

            if (File.Exists(inputPath) == false)
                return;

            var instructions = File.ReadAllLines(inputPath);

            _mem.Clear();
            _maskZeros = 0;
            _maskOnes = 0;

            for (int i = 0; i < instructions.Length; i++)
            {
                ExecuteInstruction(instructions[i]);
            }

            var totalValue = _mem.Values.Sum(x => (decimal) x);
            Console.WriteLine($"Part 1 solution is {totalValue}.");

            _mem.Clear();
            _mask = "";

            for (int i = 0; i < instructions.Length; i++)
            {
                ExecuteInstructionPart2(instructions[i]);
            }

            totalValue = _mem.Values.Sum(x => (decimal) x);
            Console.WriteLine($"Part 2 solution is {totalValue}.");
        }

        private void ExecuteInstructionPart2(string instruction)
        {
            if (instruction.Contains("mask"))
            {
                _mask = instruction.Split(" = ")[1].PadLeft(36, '0');
                return;
            }

            var match = Regex.Match(instruction, @"mem\[([0-9]+)\]");
            var memAddress = int.Parse(match.Groups[1].Value);

            var value = ulong.Parse(instruction.Split(" = ")[1]);

            foreach (var addr in Addresses(memAddress, _mask, 35))
            {
                _mem[addr] = value;
            }
        }

        private void ExecuteInstruction(string instruction)
        {
            if (instruction.Contains("mask"))
            {
                var maskStr = instruction.Split(" = ")[1];

                var maskOnesStr = maskStr.Replace('X', '0').PadLeft(36, '0');
                maskOnesStr = maskOnesStr.Trim();
                _maskOnes = Convert.ToUInt64(maskOnesStr, 2);

                var maskZerosStr = maskStr.Replace('X', '1').PadLeft(36, '1');
                _maskZeros = Convert.ToUInt64(maskZerosStr, 2);

                // Console.WriteLine($"{_maskOnes} - {_maskZeros}");
                return;
            }

            var match = Regex.Match(instruction, @"mem\[([0-9]+)\]");
            var memAddress = int.Parse(match.Groups[1].Value);

            var value = ulong.Parse(instruction.Split(" = ")[1]);

            _mem[memAddress] = (value & _maskZeros) | _maskOnes;
        }

        //Not my solution - Based on https://github.com/encse/adventofcode/blob/master/2020/Day14/Solution.cs
        private IEnumerable<long> Addresses(long baseAddr, string mask, int i)
        {
            if (i == -1)
            {
                yield return 0;
            }
            else
            {
                foreach (var prefix in Addresses(baseAddr, mask, i - 1))
                {
                    switch (mask[i])
                    {
                        case '0':
                            yield return (prefix << 1) + ((baseAddr >> 35 - i) & 1);
                            break;
                        case '1':
                            yield return (prefix << 1) + 1;
                            break;
                        default:
                            yield return prefix << 1;
                            yield return (prefix << 1) + 1;
                            break;
                    }
                }
            }
        }
    }
}