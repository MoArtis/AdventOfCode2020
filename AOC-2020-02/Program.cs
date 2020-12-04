using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC_2020_02
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

            var validPasswordsCount = 0;

            var lines = File.ReadAllLines(inputPath);
            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"\d+");
                int.TryParse(match.Value, out var policyNum1);

                match = match.NextMatch();
                int.TryParse(match.Value, out var policyNum2);

                match = Regex.Match(line, @" ([A-Za-z0-9\-]): ");
                var policyChar = match.Groups[1].Value;

                match = Regex.Match(line, @": ([A-Za-z0-9\-]+)");
                var password = match.Groups[1].Value;

                // var isValid = ValidatePasswordWithCharCountPolicy(policyNum1, policyNum2, policyChar, password);
                var isValid = ValidatePasswordWithCharPositionPolicy(policyNum1, policyNum2, policyChar[0], password);
                
                Console.Write($"{policyNum1} - {policyNum2} - {policyChar} - {password} - ");
                Console.ForegroundColor = isValid ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(isValid ? "Valid" : "Invalid");
                Console.ForegroundColor = ConsoleColor.White;

                if (isValid)
                    validPasswordsCount++;
            }

            Console.WriteLine($"The number of valid passwords is: {validPasswordsCount}");
        }

        private bool ValidatePasswordWithCharCountPolicy(int min, int max, string policyChar, string password)
        {
            // var charCount = (password.Length - password.Replace(policyChar, "").Length) / policyChar.Length;
            var charCount = Regex.Matches(password, policyChar).Count;
            return charCount >= min && charCount <= max;
        }

        private bool ValidatePasswordWithCharPositionPolicy(int pos1, int pos2, char policyChar, string password)
        {
            var passwordLength = password.Length;

            var hasCharOnPos1 = false;
            if (pos1 <= passwordLength)
            {
                if (password[pos1 - 1] == policyChar)
                    hasCharOnPos1 = true;
            }
            
            var hasCharOnPos2 = false;
            if (pos2 <= passwordLength)
            {
                if (password[pos2 - 1] == policyChar)
                    hasCharOnPos2 = true;
            }

            return hasCharOnPos1 ^ hasCharOnPos2;
        }
    }
}