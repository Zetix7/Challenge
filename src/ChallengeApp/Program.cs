using System;
using System.IO;

namespace ChallengeApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            WriteColor("Hello! It is 'Movie Rating Program' and more...\n\n", ConsoleColor.Yellow);
            var title = InsertTitleAndDirector(out string input, "Insert title of movie (ONLY LETTERS, DIGITS and SPACE (not ONLY space), default = Unknown): ");
            var director = InsertTitleAndDirector(out input, "Insert director of movie (ONLY LETTERS, DIGITS and SPACE (not ONLY space), default = Unknown): ");
            var inMemoryFilm = new InMemoryFilm(title, director);
            var savedFilm = new SavedFilm(title, director);
            Console.WriteLine();

            if(File.Exists(savedFilm.FileNameCast) && new FileInfo(savedFilm.FileNameCast).Length > 0)
            {
                AddSeparator();
                WriteColor("[Replace ONLY here] ", ConsoleColor.DarkRed);
                WriteColor($"Do you want replace director in file {savedFilm.FileNameCast} for this one: {savedFilm.Director}? (Y/N)\n", ConsoleColor.Cyan);
                WriteColor("\tYour choice: ", ConsoleColor.Yellow);
                input = Console.ReadLine().ToUpper().Trim();

                if (input == "Y")
                {
                    savedFilm.ChangeDirectorInFile(savedFilm.Director);
                }
            }

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
                        else if (input == "V")
                        {
                            SetNames(out string firstName, out string lastName);
                            
                            ModifyCast(inMemoryFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "X")
                        {
                            if (inMemoryFilm.IsEmptyCast())
                            {
                                AddSeparator();
                                WriteColor("Cast is empty! Add at least one artist!\n", ConsoleColor.DarkRed);
                                AddSeparator();
                                continue;
                            }
                            
                            SetNames(out string firstName, out string lastName);
                            
                            ModifyCast(inMemoryFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "Y")
                        {
                            AddSeparator();
                            ShowCastOrMovieDetails(inMemoryFilm, input);
                            continue;
                        }
                        else if (input == "Z")
                        {                            
                            AddSeparator();
                            ShowCastOrMovieDetails(inMemoryFilm, input);
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
                        else if (input == "V")
                        {
                            SetNames(out string firstName, out string lastName);

                            ModifyCast(savedFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "X")
                        {
                            if (!File.Exists(savedFilm.FileNameCast) || new FileInfo(savedFilm.FileNameCast).Length == 0)
                            {
                                AddSeparator();
                                WriteColor("Cast is empty! Add at least one artist!\n", ConsoleColor.DarkRed);
                                AddSeparator();
                                continue;
                            }

                            SetNames(out string firstName, out string lastName);
                            
                            ModifyCast(savedFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "Y")
                        {
                            AddSeparator();
                            ShowCastOrMovieDetails(savedFilm, input);
                            continue;
                        }
                        else if (input == "Z")
                        {
                            AddSeparator();
                            ShowCastOrMovieDetails(savedFilm, input);
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
                        else if (input == "V")
                        {
                            SetNames(out string firstName, out string lastName);
                            
                            ModifyCast(inMemoryFilm, firstName, lastName, input);
                            ModifyCast(savedFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "X")
                        {
                            if (inMemoryFilm.IsEmptyCast() && (!File.Exists(savedFilm.FileNameCast) || new FileInfo(savedFilm.FileNameCast).Length == 0))
                            {
                                AddSeparator();
                                WriteColor("Cast is empty! Add at least one artist!\n", ConsoleColor.DarkRed);
                                AddSeparator();
                                continue;
                            }

                            SetNames(out string firstName, out string lastName);

                            ModifyCast(inMemoryFilm, firstName, lastName, input);
                            ModifyCast(savedFilm, firstName, lastName, input);
                            continue;
                        }
                        else if (input == "Y")
                        {
                            AddSeparator();
                            ShowCastOrMovieDetails(inMemoryFilm, input);
                            ShowCastOrMovieDetails(savedFilm, input);
                            continue;
                        }
                        else if (input == "Z")
                        {
                            AddSeparator();
                            ShowCastOrMovieDetails(inMemoryFilm, input);
                            ShowCastOrMovieDetails(savedFilm, input);
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
            Console.WriteLine(" Q - to back previous menu.\n" +
                " S - to see current stats.\n" +
                " V - to add new artist.\n" +
                " X - to remove artist.\n" +
                " Y - to see cast.\n" +
                " Z - to see detail.\n");
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

        private static void ModifyCast(IFilm film, string firstName, string lastName, string input)
        {
            try
            {
                if(input == "V")
                {
                    film.NewArtistAdded += OnNewArtistAdded;
                    film.AddNewArtist(firstName, lastName);
                }
                else
                {
                    film.ArtistRemoved += OnArtistRemoved;
                    film.RemoveArtist(firstName, lastName);
                }
            }
            catch (FileNotFoundException fe)
            {
                WriteColor(fe.Message + "\n", ConsoleColor.DarkRed);
            }
            catch (IndexOutOfRangeException ie)
            {
                WriteColor(ie.Message + "\n", ConsoleColor.DarkRed);
            }
            catch (ArgumentException ae)
            {
                WriteColor(ae.Message + "\n", ConsoleColor.DarkRed);
            }
            catch (FormatException fe)
            {
                WriteColor(fe.Message + "\n", ConsoleColor.DarkRed);
            }
            finally
            {
                film.NewArtistAdded -= OnNewArtistAdded;
                film.ArtistRemoved -= OnArtistRemoved;
                AddSeparator();
            }
        }

        private static void SetNames(out string firstName, out string lastName)
        {
            AddSeparator();
            WriteColor("Enter first name: ", ConsoleColor.Yellow);
            firstName = Console.ReadLine();
            WriteColor("Enter last name: ", ConsoleColor.Yellow);
            lastName = Console.ReadLine();
            AddSeparator();
        }

        private static void ShowCastOrMovieDetails(IFilm film, string input)
        {
            try
            {
                film.ShowCastOrMovieDetails(input);
                WriteColor("Press any key...", ConsoleColor.Green);
                Console.ReadKey();
                Console.WriteLine();
            }
            catch (FileNotFoundException fe)
            {
                WriteColor(fe.Message + "\n", ConsoleColor.DarkRed);
            }
            catch (IndexOutOfRangeException ie)
            {
                WriteColor(ie.Message + "\n", ConsoleColor.DarkRed);
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
                if(choise != "3" || film.GetType().Equals(typeof(InMemoryFilm))) 
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

        private static string InsertTitleAndDirector(out string input, string message)
        {
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

        private static void OnArtistRemoved(object sender, EventArgs e)
        {
            if (sender.GetType().Equals(typeof(InMemoryFilm)))
            {
                WriteColor("--- Artist removed from cast.\n", ConsoleColor.Green);
            }
            else
            {
                WriteColor("--- Artist removed from file.\n", ConsoleColor.Green);
            }
        }

        private static void OnNewArtistAdded(object sender, EventArgs e)
        {
            if (sender.GetType().Equals(typeof(InMemoryFilm)))
            {
                WriteColor("--- New artist added to cast.\n", ConsoleColor.Green);
            }
            else
            {
                WriteColor("--- New artist added to file.\n", ConsoleColor.Green);
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
