using System;
using System.IO;

namespace ChallengeApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var title = InsertTitle(out string input, out string message);
            var inMemoryFilm = new InMemoryFilm(title);
            var savedFilm = new SavedFilm(title);
            Console.WriteLine();

            while (true)
            {
                AddSeparator();
                WriteColor("Where do you want save grades? Memory (1) or file (2) or both (3)? \"Q\" is exit the program.\n", ConsoleColor.Cyan);
                WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                input = Console.ReadLine().ToUpper().Trim();
                Console.WriteLine();
                AddSeparator();

                if (input == "Q")
                {
                    WriteColor(" See you later...\n", ConsoleColor.Green);
                    WriteColor(" Press any key...", ConsoleColor.Green);
                    Console.ReadKey();
                    break;
                }
                else if (input == "1")
                {
                    WriteColor("******* NOW YOU ARE SAVING GRADE TO MEMORY *******\n", ConsoleColor.Green);
                    AddSeparator();

                    while (true)
                    {
                        ShowMenu();
                        WriteColor($"\t\tIN MEMORY: {inMemoryFilm.Title}\n", ConsoleColor.Cyan);
                        WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                        input = Console.ReadLine().ToUpper().Trim();

                        if (input == "Q")
                        {
                            break;
                        }
                        else if (input == "S")
                        {
                            AddSeparator();
                            ShowStatistics(inMemoryFilm);
                            continue;
                        }

                        inMemoryFilm.GradeAdded += OnGradeAdded;
                        inMemoryFilm.GradeAddedUnder50 += OnGradeAddedUnder50;
                        AddSeparator();
                        AddNewGrade(inMemoryFilm, input);
                        AddSeparator();
                        inMemoryFilm.GradeAdded -= OnGradeAdded;
                        inMemoryFilm.GradeAddedUnder50 -= OnGradeAddedUnder50;
                    }
                }
                else if (input == "2")
                {
                    WriteColor($"******* NOW YOU ARE SAVING GRADE TO FILE '{savedFilm.FileNameGrades}' *******\n", ConsoleColor.Green);
                    AddSeparator();
                    while (true)
                    {
                        ShowMenu();
                        WriteColor($"\t\tIN FILE: {savedFilm.FileNameGrades}\n", ConsoleColor.Cyan);
                        WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                        input = Console.ReadLine().ToUpper().Trim();

                        if (input == "Q")
                        {
                            break;
                        }
                        else if (input == "S")
                        {
                            AddSeparator();
                            ShowStatistics(savedFilm);
                            continue;
                        }

                        savedFilm.GradeAdded += OnGradeAdded;
                        savedFilm.GradeAddedUnder50 += OnGradeAddedUnder50;
                        AddSeparator();
                        AddNewGrade(savedFilm, input);
                        AddSeparator();
                        savedFilm.GradeAdded -= OnGradeAdded;
                        savedFilm.GradeAddedUnder50 -= OnGradeAddedUnder50;
                    }
                }
                else if (input == "3")
                {
                    WriteColor($"******* NOW YOU ARE SAVING GRADE TO MEMORY AND FILE '{savedFilm.FileNameGrades}' *******\n", ConsoleColor.Green);
                    AddSeparator();

                    while (true)
                    {
                        ShowMenu();
                        WriteColor($"\t\tIN MEMORY: {inMemoryFilm.Title} \n\t\tIN FILE: {savedFilm.FileNameGrades}\n", ConsoleColor.Cyan);
                        WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                        input = Console.ReadLine().ToUpper().Trim();

                        if (input == "Q")
                        {
                            break;
                        }
                        else if (input == "S")
                        {
                            AddSeparator();
                            ShowStatistics(inMemoryFilm);
                            ShowStatistics(savedFilm);
                            continue;
                        }

                        inMemoryFilm.GradeAdded += OnGradeAdded;
                        inMemoryFilm.GradeAddedUnder50 += OnGradeAddedUnder50;
                        AddSeparator();
                        if (AddNewGrade(inMemoryFilm, input))
                        {
                            AddNewGrade(savedFilm, input);
                        }
                        AddSeparator();
                        inMemoryFilm.GradeAdded -= OnGradeAdded;
                        inMemoryFilm.GradeAddedUnder50 -= OnGradeAddedUnder50;
                    }
                }
                else
                {
                    WriteColor("Incorret value. Choose 1 or 2 or 3 or Q!\n", ConsoleColor.Red);
                }
            }
        }

        private static void ShowMenu()
        {
            WriteColor("Grade value must be between 0 and 100 or letter between \"A\" - \"E\" or \"A+\" - \"E+\" or \"A-\" - \"E-\".\n", ConsoleColor.Cyan);
            Console.WriteLine(" Q - to back previous menu.\n S - to see current stats.\n");
            WriteColor("Choose one option or enter grade for movie:\n", ConsoleColor.Cyan);
        }

        private static void ShowStatistics(IFilm iFilm)
        {
            try
            {
                var statsIFilm = iFilm.GetStatistics();
                if (statsIFilm.Total == 0)
                {
                    if (iFilm.GetType().Equals(typeof(InMemoryFilm)))
                    {
                        WriteColor($"IN MEMORY : Grades is empty.\n", ConsoleColor.DarkRed);
                    }
                    else
                    {
                        WriteColor($"IN FILE : Grades is empty.\n", ConsoleColor.DarkRed);
                    }
                }
                else
                {
                    if (iFilm.GetType().Equals(typeof(InMemoryFilm)))
                    {
                        Console.WriteLine($"IN MEMORY : Title: {iFilm.Title}\n\tTotal grades is {statsIFilm.Total}.");
                    }
                    else
                    {
                        Console.WriteLine($"IN FILE : Title: {iFilm.Title}\n\tTotal grades is {statsIFilm.Total}.");
                    }

                    Console.WriteLine($"\tLowest grade is {statsIFilm.Low:N2}.");
                    Console.WriteLine($"\tHighest grade is {statsIFilm.High:N2}.");
                    Console.WriteLine($"\tAverage grade is {statsIFilm.Average:N2}.");
                    Console.WriteLine($"\tLetter: {statsIFilm.Letter}");
                    WriteColor("Press any key...", ConsoleColor.Green);
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
            catch (FileNotFoundException fe)
            {
                WriteColor(fe.Message + "\n", ConsoleColor.DarkRed);
            }
            finally
            {
                AddSeparator();
            }
        }

        private static bool AddNewGrade(IFilm iFilm, string input)
        {
            var result = false;
            try
            {
                if (input.Length == 1 && char.IsLetter(input[0]))
                {
                    iFilm.AddGrade(input[0]);
                }
                else
                {
                    iFilm.AddGrade(input);
                }
                result = true;
            }
            catch (FormatException fe)
            {
                WriteColor(fe.Message + "\n", ConsoleColor.DarkRed);
            }
            catch (ArgumentException ae)
            {
                WriteColor(ae.Message + "\n", ConsoleColor.DarkRed);
            }
            return result;
        }

        private static string InsertTitle(out string input, out string message)
        {
            message = "Hello!  It is 'Movie Rating Program'\n\n"
                + "Insert title of movie (ONLY LETTERS, DIGITS and SPACE (not ONLY space), default = Unknown): ";
            WriteColor(message, ConsoleColor.Yellow);
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (var letter in input)
                {
                    if (!char.IsLetterOrDigit(letter) && letter != ' ')
                    {
                        return "Unknown";
                    }
                }
                return input;
            }
            else
            {
                return "Unknown";
            }
        }

        private static void OnGradeAddedUnder50(object sender, EventArgs args)
        {
            WriteColor("--- Grade under 50. Movie is boring.\n", ConsoleColor.Green);
        }

        private static void OnGradeAdded(object sender, EventArgs args)
        {
            WriteColor("--- New grade is added.\n", ConsoleColor.Green);
        }

        private static void AddSeparator()
        {
            Console.WriteLine("_________________________________________________________________________________________________\n");
        }

        private static void WriteColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}
