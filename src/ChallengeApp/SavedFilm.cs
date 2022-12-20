using System;
using System.Collections.Generic;
using System.IO;

namespace ChallengeApp
{
    class SavedFilm : FilmBase
    {
        private const string FILENAME_GRADES = "-Grades.txt";
        private string titleFileName;
        private readonly string fileNameGrades;

        public SavedFilm(string title) : base(title)
        {
            this.titleFileName = Title.Replace(' ', '_');
            fileNameGrades = $"{this.titleFileName}{FILENAME_GRADES}";
        }

        public string FileNameGrades
        {
            get { return fileNameGrades; }
        }

        public event GradeAddedDelegate GradeAdded;
        public event GradeAddedDelegate GradeAddedUnder50;

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
    }
}