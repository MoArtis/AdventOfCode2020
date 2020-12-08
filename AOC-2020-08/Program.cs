using System;
using System.Collections.Generic;
using System.IO;

namespace AOC_2020_08
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

        private static int _accumulator = 0;
        private static int _instructionIndex = 0;

        private class Instruction
        {
            public Instruction(Action<int> operation, int argument)
            {
                Operation = operation;
                Argument = argument;
                ExecutionCount = 0;
            }

            public Action<int> Operation;
            public int Argument;
            public int ExecutionCount;
        }

        private void Run(string inputPath)
        {
            var instructions = GenerateInstructions(inputPath);

            if (instructions == null)
                return;

            //Part 1
            RunUntilAnyInstructionIsExecutedTwice(instructions);
            Console.WriteLine($"The accumulator value before any instruction is executed twice is {_accumulator}");

            //Part 2
            if (ModifyInstructionsUntilTerminatedNormally(instructions))
                Console.WriteLine($"The accumulator value at the end of the fixed instruction set is {_accumulator}");
            else
                Console.WriteLine("The instructions cannot be fixed.");
        }

        private bool ModifyInstructionsUntilTerminatedNormally(Instruction[] instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Operation == Jump)
                    instruction.Operation = NoOperation;
                else if (instruction.Operation == NoOperation)
                    instruction.Operation = Jump;

                if (RunUntilAnyInstructionIsExecutedTwice(instructions))
                    return true;

                if (instruction.Operation == Jump)
                    instruction.Operation = NoOperation;
                else if (instruction.Operation == NoOperation)
                    instruction.Operation = Jump;
            }

            return false;
        }

        private bool RunUntilAnyInstructionIsExecutedTwice(Instruction[] instructions)
        {
            _accumulator = 0;
            _instructionIndex = 0;
            foreach (var instruction in instructions)
            {
                instruction.ExecutionCount = 0;
            }
            
            while (_instructionIndex >= 0 && _instructionIndex < instructions.Length)
            {
                var instruction = instructions[_instructionIndex];
                if (instruction.ExecutionCount >= 1)
                    return false;

                instruction.Operation.Invoke(instruction.Argument);
                instruction.ExecutionCount++;
            }

            return true;
        }

        private Instruction[] GenerateInstructions(string inputPath)
        {
            if (!File.Exists(inputPath))
                return null;

            var instructions = new List<Instruction>();

            var sr = new StreamReader(inputPath);
            using (sr)
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var splitLine = line.Split(" ");

                    if (int.TryParse(splitLine[1], out var argument) == false)
                    {
                        Console.WriteLine($"Can't parse the argument of instruction {line}");
                        return null;
                    }

                    Action<int> operation = splitLine[0] switch
                    {
                        "acc" => Accumulate,
                        "jmp" => Jump,
                        _ => NoOperation
                    };

                    instructions.Add(new Instruction(operation, argument));
                }
            }

            return instructions.ToArray();
        }

        private void Accumulate(int argument)
        {
            _accumulator += argument;
            _instructionIndex++;
        }

        private void Jump(int argument)
        {
            _instructionIndex += argument;
        }

        private void NoOperation(int argument)
        {
            _instructionIndex++;
        }
    }
}