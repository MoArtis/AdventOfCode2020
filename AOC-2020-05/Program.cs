using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AOC_2020_05
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "./input.txt";
            // const string inputPath = "./testInput.txt";

            var program = new Program();
            program.Run(inputPath);
        }

        private void Run(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return;

            var sr = new StreamReader(inputPath);
            var seatIds = new List<int>();
            using (sr)
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var row = FromBspCodeToRow(line);
                    var column = FromBspCodeToColumn(line);

                    var seatId = ComputeSeatId(row, column);

                    seatIds.Add(seatId);
                    Console.WriteLine($"{line} - R:{row} - C:{column} - sid:{seatId}");
                }
            }

            var minSeatId = seatIds.Min();
            var maxSeatId = seatIds.Max();

            //Part 1
            Console.WriteLine($"The highest seat ID is {seatIds.Max()}");

            //Part 2
            var missingSeatId = ComputeMissingSeatId(minSeatId, maxSeatId, seatIds.Sum());
            Console.WriteLine($"The missing seat ID is {missingSeatId}");
        }

        private int ComputeMissingSeatId(int minSeatId, int maxSeatId, int sumSeatId)
        {
            minSeatId--;
            var zeroToMinTotal = minSeatId * (minSeatId + 1) / 2;
            sumSeatId += zeroToMinTotal;

            var maxTotal = maxSeatId * (maxSeatId + 1) / 2;

            return maxTotal - sumSeatId;
        }

        private int ComputeSeatId(int row, int column)
        {
            return row * 8 + column;
        }

        private int FromBspCodeToRow(string bspCode)
        {
            if (bspCode.Length < 7)
                return -1;

            var row = 0;
            for (var i = 0; i < 7; i++)
            {
                var letter = bspCode[i];
                if (letter == 'B')
                {
                    row += 128 / (int) MathF.Pow(2, i + 1);
                    continue;
                }

                if (letter != 'F')
                    return -1;
            }

            return row;
        }

        private int FromBspCodeToColumn(string bspCode)
        {
            if (bspCode.Length < 10)
                return -1;

            var column = 0;
            for (var i = 0; i < 3; i++)
            {
                var letter = bspCode[i + 7];
                if (letter == 'R')
                {
                    column += 8 / (int) MathF.Pow(2, i + 1);
                    continue;
                }

                if (letter != 'L')
                    return -1;
            }

            return column;
        }
    }
}