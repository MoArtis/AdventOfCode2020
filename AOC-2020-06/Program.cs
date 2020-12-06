using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC_2020_06
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "./input.txt";
            // const string inputPath = "./testInput.txt";

            var customFormProcessor = new CustomFormProcessor();
            customFormProcessor.ProcessInput(inputPath);
        }
    }

    internal class CustomFormProcessor
    {
        public void ProcessInput(string inputPath)
        {
            if (File.Exists(inputPath) == false)
                return;

            var groupsAnswers = new List<string>();
            
            var sr = new StreamReader(inputPath);
            using (sr)
            {
                var answersSb = new StringBuilder();
                var isEndOfFile = false;
                while (!isEndOfFile)
                {
                    // var line = sr.ReadLineAsync();
                    var line = sr.ReadLine();
                    isEndOfFile = line == null;

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        groupsAnswers.Add(answersSb.ToString());
                        answersSb.Clear();
                        continue;
                    }

                    answersSb.Append(line);
                    answersSb.Append(";");
                }
            }

            var uniqueAnswersSum = SumUniqueGroupsAnswers(groupsAnswers);
            
            //Part 1
            Console.WriteLine($"the sum of unique answers is {uniqueAnswersSum}.");
            // Console.WriteLine($"the sum of these counts is {string.Join(" + ", groupsAnswers.Select(x => x.Length).ToArray())} = {sum}.");
            
            //Part 2
            var allPositiveAnswersSum = SumAllPositiveGroupAnswers(groupsAnswers);
            Console.WriteLine($"the sum of all positive answers is {allPositiveAnswersSum}.");
        }

        private int SumUniqueGroupsAnswers(List<string> groupsAnswers)
        {
            return groupsAnswers.Sum(t => t.Replace(";", "").Distinct().Count());
            // return groupsAnswers.Sum(t => t.Replace(";", "").Distinct().Count() - 1);
            // return string.Join("", groupsAnswers.Select(x => new string(x.Distinct().ToArray()))).Length;
        }

        private int SumAllPositiveGroupAnswers(List<string> groupsAnswers)
        {
            var total = 0;
            for (var i = 0; i < groupsAnswers.Count; i++)
            {
                var groupAnswer = groupsAnswers[i];
                var groupSize = groupAnswer.Count(x => x == ';');
                groupAnswer = groupAnswer.Replace(";", "");
                var test = groupAnswer.GroupBy(x => x);
                total += test.Count(x=> x.Count() == groupSize);
            }
            return total;
        }
    }
}