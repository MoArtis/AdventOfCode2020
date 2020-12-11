using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC_2020_11
{
    class Program
    {
        private (int, int)[] _adjacentOffsets = {(1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1)};

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            var inputPath = "./input.txt";
            // var inputPath = "./testInput.txt";

            var chairMap = ParseChairMap(inputPath);

            if (chairMap == null)
                return;

            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);
            var workingChairMap = new char[width, height];

            UpdateChairMap(chairMap, workingChairMap);
            Part1(workingChairMap);

            UpdateChairMap(chairMap, workingChairMap);
            Part2(workingChairMap);
        }

        private void Part2(char[,] chairMap)
        {
            if (chairMap == null)
                return;

            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);
            var previousChairMap = new char[width, height];

            var roundCount = 0;
            while (roundCount < 1000)
            {
                UpdateChairMap(chairMap, previousChairMap);

                var hasChanged = ApplyRound(chairMap, previousChairMap, ApplyRoundOnCoordWithLos);
                roundCount++;

                if (hasChanged == false)
                    break;
            }

            var occupiedChairCount = CountChairState(chairMap, '#');
            Console.WriteLine($"Part 2 - Round {roundCount} - The number of occupied chair is {occupiedChairCount}.");
        }

        private void Part1(char[,] chairMap)
        {
            if (chairMap == null)
                return;

            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);
            var previousChairMap = new char[width, height];

            var roundCount = 0;
            while (roundCount < 1000)
            {
                UpdateChairMap(chairMap, previousChairMap);

                var hasChanged = ApplyRound(chairMap, previousChairMap, ApplyRoundOnCoord);
                roundCount++;

                if (hasChanged == false)
                    break;
            }

            var occupiedChairCount = CountChairState(chairMap, '#');
            Console.WriteLine($"Part 1 - Round {roundCount} - The number of occupied chair is {occupiedChairCount}.");
        }

        private int CountChairState(char[,] chairMap, char state)
        {
            var count = 0;
            foreach (var chairMapState in chairMap)
            {
                if (chairMapState == state)
                    count++;
            }

            return count;
        }

        private void UpdateChairMap(char[,] from, char[,] to)
        {
            var width = from.GetLength(0);
            var height = from.GetLength(1);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    to[x, y] = from[x, y];
                }
            }
        }

        private delegate char ApplyRoundOnCoordDelegate(char[,] chairMap, int x, int y);

        private bool ApplyRound(char[,] chairMap, char[,] previousChairMap, ApplyRoundOnCoordDelegate applyRoundOnCoord)
        {
            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);

            var changeCount = 0;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var newState = applyRoundOnCoord.Invoke(previousChairMap, x, y);

                    if (newState == chairMap[x, y]) continue;

                    chairMap[x, y] = newState;
                    changeCount++;
                }
            }

            return changeCount > 0;
        }


        private char ApplyRoundOnCoordWithLos(char[,] chairMap, int x, int y)
        {
            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);

            if (!HasCoord(x, y, width, height))
            {
                Console.WriteLine($"Error - The coordinate {x}|{y} doesn't exist.");
                return '?';
            }

            var state = chairMap[x, y];

            switch (state)
            {
                case 'L':
                {
                    foreach (var (dirX, dirY) in _adjacentOffsets)
                    {
                        var targetX = x;
                        var targetY = y;

                        while (true)
                        {
                            targetX += dirX;
                            targetY += dirY;

                            if (!HasCoord(targetX, targetY, width, height))
                                break;
                            
                            if (chairMap[targetX, targetY] == 'L')
                                break;

                            if (chairMap[targetX, targetY] == '#')
                                return state;
                        }
                    }

                    return '#';
                }

                case '#':
                {
                    var adjOccupiedCount = 0;
                    foreach (var (dirX, dirY) in _adjacentOffsets)
                    {
                        var targetX = x;
                        var targetY = y;

                        while (true)
                        {
                            targetX += dirX;
                            targetY += dirY;

                            if (!HasCoord(targetX, targetY, width, height))
                                break;

                            if (chairMap[targetX, targetY] == 'L')
                                break;

                            if (chairMap[targetX, targetY] != '#') continue;

                            adjOccupiedCount++;
                            break;
                        }
                    }

                    return adjOccupiedCount >= 5 ? 'L' : state;
                }

                default:
                    return state;
            }
        }


        private char ApplyRoundOnCoord(char[,] chairMap, int x, int y)
        {
            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);

            if (!HasCoord(x, y, width, height))
            {
                Console.WriteLine($"Error - The coordinate {x}|{y} doesn't exist.");
                return '?';
            }

            var state = chairMap[x, y];

            switch (state)
            {
                case 'L':
                {
                    foreach (var (adjOffsetX, adjOffsetY) in _adjacentOffsets)
                    {
                        var adjX = adjOffsetX + x;
                        var adjY = adjOffsetY + y;

                        if (!HasCoord(adjX, adjY, width, height))
                            continue;

                        if (chairMap[adjX, adjY] == '#')
                            return state;
                    }

                    return '#';
                }

                case '#':
                {
                    var adjOccupiedCount = 0;
                    foreach (var (adjOffsetX, adjOffsetY) in _adjacentOffsets)
                    {
                        var adjX = adjOffsetX + x;
                        var adjY = adjOffsetY + y;

                        if (!HasCoord(adjX, adjY, width, height))
                            continue;

                        if (chairMap[adjX, adjY] == '#')
                            adjOccupiedCount++;
                    }

                    return adjOccupiedCount >= 4 ? 'L' : state;
                }

                default:
                    return state;
            }
        }

        private char[,] ParseChairMap(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return null;

            var lines = File.ReadAllLines(inputPath);

            if (lines.Length <= 0 || lines[0].Length <= 0)
                return null;

            Array.Reverse(lines);

            var chairMap = new char[lines[0].Length, lines.Length];

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    chairMap[x, y] = line[x];
                }
            }

            return chairMap;
        }

        private void DisplayChairMap(char[,] chairMap)
        {
            var width = chairMap.GetLength(0);
            var height = chairMap.GetLength(1);

            var sb = new StringBuilder();
            for (var y = height - 1; y >= 0; y--)
            {
                for (var x = 0; x < width; x++)
                {
                    sb.Append(chairMap[x, y]);
                }

                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        private bool HasCoord(int x, int y, int width, int height)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }
    }
}