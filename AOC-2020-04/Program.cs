using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC_2020_04
{
    class Program
    {
        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            const string inputPath = "./input.txt";
            // const string inputPath = "./testInput.txt";
            // const string inputPath = "./validPassports.txt";
            // const string inputPath = "./invalidPassports.txt";

            if (File.Exists(inputPath) == false)
                return;

            var sr = new StreamReader(inputPath);
            using (sr)
            {
                var sb = new StringBuilder();
                var isEndOfFile = false;
                var passportWithRequiredFieldsCount = 0;
                var validPassportCount = 0;
                var line = "";

                while (!isEndOfFile)
                {
                    isEndOfFile = (line = sr.ReadLine()) == null;

                    if (isEndOfFile || string.IsNullOrEmpty(line))
                    {
                        var passportTextData = sb.ToString();
                        Console.WriteLine(passportTextData);
                        if (HasRequiredFields(passportTextData))
                        {
                            passportWithRequiredFieldsCount++;

                            var isValid = IsValid(passportTextData);
                            
                            Console.ForegroundColor = isValid ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.WriteLine(isValid? "Passport is valid!" : "Passport is invalid...");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            
                            if(isValid)
                                validPassportCount++;
                            
                        }

                        sb.Clear();
                        continue;
                    }

                    sb.Append(line);
                    sb.Append(" ");
                }

                Console.WriteLine($"The number of passport with all required fields is {passportWithRequiredFieldsCount}");
                Console.WriteLine($"The number of valid passport is {validPassportCount}");
            }
        }

        private static readonly string[] RequiredFields =
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid"
            // "cid"
        };

        private static readonly string[] eyeColors =
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth"
        };

        private bool HasRequiredFields(string passportTextData)
        {
            var hasRequiredFields = RequiredFields.All(passportTextData.Contains);

            Console.ForegroundColor = hasRequiredFields ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(hasRequiredFields? "Passport has all required fields!" : "Passport has missing fields");
            Console.ForegroundColor = ConsoleColor.White;

            return hasRequiredFields;
        }

        private bool IsValid(string passportTextData)
        {
            passportTextData = passportTextData.Trim();
            var keyValuePairs = passportTextData.Split(" ");
            var passportData = new Dictionary<string, string>();
            for (var i = 0; i < keyValuePairs.Length; i++)
            {
                var keyValuePair = keyValuePairs[i].Split(":");
                passportData.Add(keyValuePair[0], keyValuePair[1]);
            }

            //Birth year
            if (ValidateYearData(passportData["byr"], 1920, 2002) == false)
                return false;

            //Issue year
            if (ValidateYearData(passportData["iyr"], 2010, 2020) == false)
                return false;

            //Expiration year
            if (ValidateYearData(passportData["eyr"], 2020, 2030) == false)
                return false;

            //Height
            if (ValidateHeight(passportData["hgt"]) == false)
                return false;

            //Hair color
            if (Regex.IsMatch(passportData["hcl"], @"^#[\da-f]{6}$") == false)
                return false;

            //Eye color
            if (eyeColors.Any(passportData["ecl"].Contains) == false)
                return false;

            //Passport ID
            if (Regex.IsMatch(passportData["pid"], @"^\d{9}$") == false)
                return false;

            return true;
        }

        private bool ValidateHeight(string heightTextData)
        {
            var measure = heightTextData.Substring(heightTextData.Length - 2);
            switch (measure)
            {
                case "cm":
                {
                    heightTextData = heightTextData.Remove(heightTextData.Length - 2);
                    if (int.TryParse(heightTextData, out var height) == false)
                        return false;

                    return height >= 150 && height <= 193;
                }
                case "in":
                {
                    heightTextData = heightTextData.Remove(heightTextData.Length - 2);
                    if (int.TryParse(heightTextData, out var height) == false)
                        return false;

                    return height >= 59 && height <= 76;
                }
                default:
                    return false;
            }
        }

        private bool ValidateYearData(string yearTextData, int min, int max)
        {
            if (int.TryParse(yearTextData, out var year) == false)
                return false;

            return year >= min && year <= max;
        }
    }
}