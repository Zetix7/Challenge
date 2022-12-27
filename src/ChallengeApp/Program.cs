using System;
using System.IO;

namespace ChallengeApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var title = InsertTitle();
            var inMemoryFilm = new InMemoryFilm(title);
            var savedFilm = new SavedFilm(title);
            Console.WriteLine();

            while (true)
            {
                AddSeparator();
                WriteColor("Where do you want save grades? Memory (1) or file (2) or both (3)? \"Q\" is exit the program.\n", ConsoleColor.Cyan);
                WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                var input = Console.ReadLine().ToUpper().Trim();
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

                        AddSeparator();
                        AddNewGrade(inMemoryFilm, input, "");
                        AddSeparator();
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

                        AddSeparator();
                        AddNewGrade(savedFilm, input, "");
                        AddSeparator();
                    }
                }
                else if (input == "3")
                {
                    WriteColor($"******* NOW YOU ARE SAVING GRADE TO MEMORY AND FILE '{savedFilm.FileNameGrades}' *******\n", ConsoleColor.Green);
                    AddSeparator();

                    var choise = input;

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

                        AddSeparator();
                        if (AddNewGrade(inMemoryFilm, input, choise))
                        {
                            AddNewGrade(savedFilm, input, choise);
                        }
                        AddSeparator();
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

        private static void ShowStatistics(IFilm film)
        {
            try
            {
                var statsFilm = film.GetStatistics();
                if (statsFilm.Total == 0)
                {
                    if (film.GetType().Equals(typeof(InMemoryFilm)))
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
                    if (film.GetType().Equals(typeof(InMemoryFilm)))
                    {
                        Console.WriteLine($"IN MEMORY : Title: {film.Title}\n\tTotal grades is {statsFilm.Total}.");
                    }
                    else
                    {
                        Console.WriteLine($"IN FILE : Title: {film.Title}\n\tTotal grades is {statsFilm.Total}.");
                    }

                    Console.WriteLine($"\tLowest grade is {statsFilm.Low:N2}.");
                    Console.WriteLine($"\tHighest grade is {statsFilm.High:N2}.");
                    Console.WriteLine($"\tAverage grade is {statsFilm.Average:N2}.");
                    Console.WriteLine($"\tLetter: {statsFilm.Letter}");
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

        private static bool AddNewGrade(IFilm film, string input, string choise)
        {
            var result = false;
            try
            {
                if (choise != "3" || film.GetType().Equals(typeof(InMemoryFilm)))
                {
                    film.GradeAdded += OnGradeAdded;
                    film.GradeAddedUnder50 += OnGradeAddedUnder50;
                }
                if (input.Length == 1 && char.IsLetter(input[0]))
                {
                    film.AddGrade(input[0]);
                }
                else
                {
                    film.AddGrade(input);
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
            finally 
            {
                film.GradeAdded -= OnGradeAdded;
                film.GradeAddedUnder50 -= OnGradeAddedUnder50;
            }
            return result;
        }

        private static string InsertTitle()
        {
            WriteColor("Hello! It is 'Movie Rating Program'\n\n"
                + "Insert title of movie (ONLY LETTERS, DIGITS and SPACE (not ONLY space), default = Unknown): ", ConsoleColor.Yellow);
            var input = Console.ReadLine();
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
