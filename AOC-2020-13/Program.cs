using System;
using System.IO;
using System.Linq;

namespace AOC_2020_13
{
    class Program
    {
        private static void Main()
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            var inputPath = "./input.txt";
            // var inputPath = "./testInput.txt";

            ParseInput(inputPath, out var departureTime, out var busIds);

            Part1(departureTime, busIds);
            Part2(busIds);
        }

        //https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        private static long ChineseRemainder(long[] n, long[] a)
        {
            var prod = n.Aggregate(1L, (i, j) => i * j);
            var sum = 0L;
            var length = (long) n.Length;
            for (var i = 0L; i < length; i++)
            {
                var p = prod / n[i];
                sum += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
            }

            return sum % prod;
        }

        private static long ModularMultiplicativeInverse(long a, long mod)
        {
            var b = a % mod;
            for (var x = 1; x < mod; x++)
            {
                if (b * x % mod == 1)
                {
                    return x;
                }
            }

            return 1;
        }

        private void Part2((int period, int delay)[] busIds)
        {
            var n = busIds.Select(x => (long)x.period).ToArray();
            var a = busIds.Select(x => (long)(x.period - x.delay)).ToArray();
            var result = ChineseRemainder(n, a);
            Console.WriteLine($"Part 2 answer is: {result}");
        }

        private void Part1(int departureTime, (int period, int delay)[] busIds)
        {
            var smallestWaitTime = int.MaxValue;
            var smallestWaitTimeBusId = -1;
            foreach (var (period, delay) in busIds)
            {
                var waitTime = period - departureTime % period;

                var isExactlyOnTime = waitTime == period;
                if (isExactlyOnTime || waitTime >= smallestWaitTime) continue;

                smallestWaitTime = waitTime;
                smallestWaitTimeBusId = period;
            }

            Console.WriteLine($"Part 1 answer is: {smallestWaitTime * smallestWaitTimeBusId}");
        }

        private void ParseInput(string inputPath, out int departureTime, out (int, int)[] busIds)
        {
            departureTime = 0;
            busIds = null;

            if (File.Exists(inputPath) == false)
                return;

            var lines = File.ReadAllLines(inputPath);

            if (int.TryParse(lines[0], out departureTime) == false)
                return;

            var busIdsLine = lines[1].Split(',');
            busIds = busIdsLine.Select((value, index) => (value, index))
                .Where(x => x.value != "x")
                .Select(x => (int.Parse(x.value), x.index)).ToArray();
        }
    }
}