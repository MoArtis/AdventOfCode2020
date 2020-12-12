using System;
using System.Collections.Generic;
using System.IO;

namespace AOC_2020_12
{
    class Program
    {
        private Coord[] _directions = {(1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1)};

        private Dictionary<char, int> _cardinalToDirIndex = new Dictionary<char, int>
            {{'E', 0}, {'S', 2}, {'W', 4}, {'N', 6}};

        private static int Repeat(int t, int length)
        {
            t = (int) (t - MathF.Floor(t / (float) length) * length);
            return Math.Clamp(t, 0, length);
        }

        private static void Main()
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            var inputPath = "./input.txt";
            // var inputPath = "./testInput.txt";

            var route = ParseRoute(inputPath);

            //Part 1
            var manDistance = ComputeRouteManhattanDistance(route);
            Console.WriteLine($"The manhattan distance for part 1 is : {manDistance}");

            //Part 2
            manDistance = ComputeRouteManhattanDistancePart2(route, (10, 1));
            Console.WriteLine($"The manhattan distance for part 2 is : {manDistance}");
        }

        private int ComputeRouteManhattanDistancePart2((char, int)[] route, Coord startWpPosition)
        {
            var position = new Coord(0, 0);
            var wpPosition = startWpPosition;

            foreach (var (instruction, value) in route)
            {
                switch (instruction)
                {
                    case 'F':
                        position += wpPosition * value;
                        break;

                    case 'L':
                        wpPosition.Rotate(Repeat(value / 90, 4));
                        break;

                    case 'R':
                        wpPosition.Rotate(Repeat(-value / 90, 4));
                        break;

                    default:
                        var dirIndex = _cardinalToDirIndex[instruction];
                        wpPosition += _directions[dirIndex] * value;
                        break;
                }

                // Console.WriteLine($"{instruction}{value} => Pos: ({position.x}|{position.y}) - WpPos: ({wpPosition.x}|{wpPosition.y})");
            }

            return Math.Abs(position.x) + Math.Abs(position.y);
        }

        private int ComputeRouteManhattanDistance((char, int)[] route)
        {
            var position = new Coord(0, 0);
            var direction = 0;

            foreach (var (instruction, value) in route)
            {
                switch (instruction)
                {
                    case 'F':
                        position += _directions[direction] * value;
                        break;

                    case 'L':
                        direction = Repeat(direction - value / 45, _directions.Length);
                        break;

                    case 'R':
                        direction = Repeat(direction + value / 45, _directions.Length);
                        break;

                    default:
                        var dirIndex = _cardinalToDirIndex[instruction];
                        position += _directions[dirIndex] * value;
                        break;
                }

                // Console.WriteLine($"{instruction}{value} => Pos: ({position.x}|{position.y}) - Dir: {direction}");
            }

            return Math.Abs(position.x) + Math.Abs(position.y);
        }

        private (char, int)[] ParseRoute(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return null;

            var lines = File.ReadAllLines(inputPath);

            var route = new (char, int)[lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                route[i] = (line[0], int.Parse(line.Remove(0, 1)));
            }

            return route;
        }
    }
}