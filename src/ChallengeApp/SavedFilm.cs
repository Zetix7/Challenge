using System;
using System.Collections.Generic;
using System.IO;

namespace ChallengeApp
{
    class SavedFilm : FilmBase
    {
        private const string FILENAME_GRADES = "-Grades.txt";
        private const string FILENAME_CAST = "-Cast.txt";
        private string titleFileName;
        private readonly string fileNameGrades;
        private readonly string fileNameCast;

        public SavedFilm(string title, string director) : base(title, director)
        {
            this.titleFileName = Title.Replace(' ', '_');
            fileNameGrades = $"{this.titleFileName}{FILENAME_GRADES}";
            fileNameCast = $"{this.titleFileName}{FILENAME_CAST}";
        }

        public string FileNameGrades
        {
            get { return fileNameGrades; }
        }

        public string FileNameCast
        {
            get { return fileNameCast; }
        }

        public event GradeAddedDelegate GradeAdded;
        public event GradeAddedDelegate GradeAddedUnder50;
        public event NewArtistAddedDelegate NewArtistAdded;
        public event ArtistRemovedDelegate ArtistRemoved;

        public override void AddGrade(double grade)
        {
            using (var writer = File.AppendText(fileNameGrades))
            {
                writer.WriteLine($"{grade:N2}");
            }

            using (var writerAudit = File.AppendText("audit.txt"))
            {
                writerAudit.WriteLine($"Grade: {grade,6:N2} | Date: {DateTime.Now} | File: {fileNameGrades}");
            }

            if (GradeAdded != null)
            {
                GradeAdded(this, new EventArgs());
            }

            if (GradeAddedUnder50 != null && grade < 50)
            {
                GradeAddedUnder50(this, new EventArgs());
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            if (!File.Exists(fileNameGrades))
            {
                throw new FileNotFoundException($"File '{fileNameGrades}' not exist.");
            }
            else
            {
                using (var reader = File.OpenText($"{fileNameGrades}"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var number = double.Parse(line);
                        result.Add(number);
                        line = reader.ReadLine();
                    }
                }
                return result;
            }
        }

        public override void AddNewArtist(string firstName, string lastName)
        {
            CheckFormatNames(firstName, lastName);
            string artist = $"{PascalCaseFormat(firstName)} {PascalCaseFormat(lastName)}";

            if (!File.Exists(fileNameCast) || (File.Exists(fileNameCast) && new FileInfo(fileNameCast).Length == 0))
            {
                using (var writer = File.AppendText($"{fileNameCast}"))
                {
                    writer.WriteLine($"Director: {Director}");
                    writer.WriteLine($"\t{artist}");
                }
            }
            else
            {
                var lines = new List<string>();
                using (var reader = File.OpenText($"{fileNameCast}"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        if(line.Trim().Equals(artist))
                        {
                            throw new ArgumentException("Artist exist in file!");
                        }
                        lines.Add(line);
                        line = reader.ReadLine();
                    }
                }
                using (var writer = File.AppendText($"{fileNameCast}"))
                {
                    writer.WriteLine($"\t{artist}");

                }
            }

            if (NewArtistAdded != null)
            {
                NewArtistAdded(this, new EventArgs());
            }
        }

        public override void RemoveArtist(string firstName, string lastName)
        {
            CheckFormatNames(firstName, lastName);
            string artist = $"{PascalCaseFormat(firstName)} {PascalCaseFormat(lastName)}";

            if (!File.Exists(fileNameCast))
            {
                throw new FileNotFoundException($"File '{fileNameCast}' not exist!");
            }
            else if (new FileInfo(fileNameCast).Length == 0)
            {
                throw new IndexOutOfRangeException($"File '{fileNameCast}' is empty!");
            }
            else
            {
                var lines = new List<string>();
                using (var reader = File.OpenText($"{fileNameCast}"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line.Trim().Equals(artist))
                        {
                            line = reader.ReadLine();
                            continue;
                        }
                        lines.Add(line);
                        line = reader.ReadLine();
                    }
                }

                var fileLines = File.ReadAllLines(fileNameCast);
                if (lines.Count.Equals(File.ReadAllLines(fileNameCast).Length))
                {
                    throw new ArgumentException("Artist not exist in file!");
                }

                using (var writer = File.CreateText($"{fileNameCast}"))
                {
                    foreach (var line in lines)
                    {
                        if(lines.Count == 1)
                        {
                            break;
                        }
                        writer.WriteLine(line);
                    }
                }

                if (ArtistRemoved != null)
                {
                    ArtistRemoved(this, new EventArgs());
                }
            }
        }

        public override void ShowCastOrMovieDetails(string input)
        {
            if (!File.Exists(fileNameCast))
            {
                throw new FileNotFoundException($"IN FILE : File '{fileNameCast}' not exist!");
            }
            else if (new FileInfo(fileNameCast).Length == 0)
            {
                throw new IndexOutOfRangeException($"IN FILE : File '{fileNameCast}' is empty!");
            }

            if (input == "Z")
            {
                Console.WriteLine($"IN FILE : Details of movie '{Title}'");
                Console.WriteLine($"\tTitle: {Title}");
                Console.WriteLine($"\tDirector: {Director}");
            }

            var index = 0;
            Console.WriteLine($"IN FILE : Full cast of movie '{Title}':");
            using (var reader = File.OpenText(fileNameCast))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    Console.WriteLine($"\t{index++} : {line.Trim()}");
                    line = reader.ReadLine();
                }
            }
        }

        public void ChangeDirectorInFile(string director)
        {
            PascalCaseFormat(director);
            var lines = new List<string>();
            var directorFile = "";
            using(var reader = File.OpenText(fileNameCast))
            {
                var line = reader.ReadLine();
                directorFile = line.Substring(10);
                if(director != directorFile)
                {
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        lines.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }

            if (director != directorFile)
            {
                using (var writer = File.CreateText(fileNameCast))
                {
                    writer.WriteLine($"Director: {director}");
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}