using System;
using System.Collections.Generic;
using static ChallengeApp.IFilm;

namespace ChallengeApp
{
    public abstract class FilmBase : IFilm
    {
        private static readonly Dictionary<char, int> rating = new()
        {
            { 'A', 90 },
            { 'B', 70 },
            { 'C', 50 },
            { 'D', 30 },
            { 'E', 10 }
        };

        public FilmBase(string title)
        {
            this.Title = PascalCaseFormat(title);
        }

        public abstract event GradeAddedDelegate GradeAdded;
        public abstract event GradeAddedDelegate GradeAddedUnder50;

        public string Title { get; set; }

        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
        
        public void AddGrade(string grade)
        {
            if (double.TryParse(grade, out double result))
            {
                if (result >= 0 && result <= 100)
                {
                    AddGrade(result);
                }
                else
                {
                    throw new ArgumentException("Enter grade between 0 and 100!");
                }
            }
            else if (grade.Length == 2 && (grade[1].Equals('-') || grade[1].Equals('+')))
            {
                switch (grade[1])
                {
                    case '-':
                        if (IsLetterOneOfGrade(char.ToUpper(grade[0]), 'A', 'E'))
                        {
                            AddGrade(rating.GetValueOrDefault(char.ToUpper(grade[0])) - 5);
                        }
                        else
                        {
                            throw new ArgumentException("Enter grade between \"A+\" - \"E+\" or \"A-\" - \"E-\".");
                        }
                        break;
                    case '+':
                        if (IsLetterOneOfGrade(char.ToUpper(grade[0]), 'A', 'E'))
                        {
                            AddGrade(rating.GetValueOrDefault(char.ToUpper(grade[0])) + 5);
                        }
                        else
                        {
                            throw new ArgumentException("Enter grade between \"A+\" - \"E+\" or \"A-\" - \"E-\".");
                        }
                        break;
                }
            }
            else
            {
                throw new FormatException("Grade is not a number!");
            }
        }

        public void AddGrade(char grade)
        {
            switch (grade)
            {
                case 'A':
                    this.AddGrade(90);
                    break;
                case 'B':
                    this.AddGrade(70);
                    break;
                case 'C':
                    this.AddGrade(50);
                    break;
                case 'D':
                    this.AddGrade(30);
                    break;
                case 'E':
                    this.AddGrade(10);
                    break;
                default:
                    throw new ArgumentException("Enter value between A and E!");
            }
        }

        protected static string PascalCaseFormat(string name)
        {
            if (name == "Unknown")
            {
                return name;
            }
            var result = "";

            for (var index = 0; index < name.Length; index++)
            {
                if (char.IsLetterOrDigit(name[index]))
                {
                    result += name[index];
                }
                else if (index + 1 < name.Length && char.IsLetterOrDigit(name[index + 1]))
                {
                    result += name[index];
                }
            }

            result = result.Trim();
            var names = result.Split(' ');
            result = "";

            foreach (var partName in names)
            {
                result += $"{char.ToUpper(partName[0])}{partName.Substring(1, partName.Length - 1).ToLower()} ";
            }
            return result.Trim();
        }

        protected static bool IsLetterOneOfGrade(char letter, char letterStart, char letterEnd)
        {
            for (var i = letterStart; i <= letterEnd; i++)
                if (i == letter)
                    return true;
            return false;
        }
    }
}