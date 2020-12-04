using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOC_2020_03
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
            const string inputPath = "./input.txt";
            // const string inputPath = "./testInput.txt";

            // var lines = await ReadFile(inputPath);
            var lines = File.ReadAllLines(inputPath);

            bool[,] map = null;
            
            BuildMap(lines, ref map);

            long result = 1;

            var slopes = new[]
            {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(3, 1),
                new Tuple<int, int>(5, 1),
                new Tuple<int, int>(7, 1),
                new Tuple<int, int>(1, 2)
            };

            for (var i = 0; i < slopes.Length; i++)
            {
                var (stepsX, stepsY) = slopes[i];
                var treeCount = CountTrees(map, stepsX, stepsY, 0, 0);
                result *= treeCount;
                Console.WriteLine($"{i} - {treeCount}");
            }

            Console.WriteLine($"The result is {result}");
        }

        private int CountTrees(bool[,] map, int stepsX, int stepsY, int startX, int startY)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);

            if (stepsY < 1)
                return 0;

            var currentX = startX;
            var currentY = startY;

            currentX += stepsX;
            currentY += stepsY;

            var treeCount = 0;
            while (currentY < mapHeight)
            {
                currentX = Repeat(currentX, mapWidth);
                
                if (map[currentX, currentY])
                    treeCount++;
                
                currentX += stepsX;
                currentY += stepsY;
            }

            return treeCount;
        }

        public static int Repeat(int t, int length)
        {
            if (t < 0)
                t = length - 1;

            return t % length;
        }
        
        private void BuildMap(string[] mapData, ref bool[,] map)
        {
            var width = mapData[0].Length;
            var height = mapData.Length;

            if (width <= 0 || height <= 0)
                return;

            map = new bool[width, height];
            for (var y = 0; y < height; y++)
            {
                var line = mapData[y];
                for (var x = 0; x < width; x++)
                {
                    map[x, y] = line[x] == '#';
                }
            }
        }

        private async Task<List<string>> ReadFile(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return null;

            var lines = new List<string>();

            using var sr = new StreamReader(inputPath);
            using var progressBar = new ProgressBar();

            var fi = new FileInfo(inputPath);

            var linesByteSize = 0;
            while (sr.EndOfStream == false)
            {
                var line = await sr.ReadLineAsync();

                if (string.IsNullOrEmpty(line))
                    continue;

                // var lineSize = line?.Length * sizeof(char) ?? 0;
                var lineSize = System.Text.Encoding.Default.GetByteCount(line);

                linesByteSize += lineSize;

                progressBar.Report((double) linesByteSize / fi.Length);

                lines.Add(line);
            }

            progressBar.Dispose();

            return lines;
        }
    }
}