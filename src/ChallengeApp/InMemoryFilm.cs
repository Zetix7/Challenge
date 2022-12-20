using System;
using System.Collections.Generic;

namespace ChallengeApp
{
    public class InMemoryFilm : FilmBase
    {
        private List<double> grades = new();
        private List<string> cast = new();

        public InMemoryFilm(string title, string director) : base(title, director) { }

        public event GradeAddedDelegate GradeAdded;
        public event GradeAddedDelegate GradeAddedUnder50;
        public event NewArtistAddedDelegate NewArtistAdded;
        public event ArtistRemovedDelegate ArtistRemoved;

        public override void AddGrade(double grade)
        {
            if (grade >= 0 && grade <= 100)
            {
                this.grades.Add(grade);
                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }

                if (GradeAddedUnder50 != null && grade < 50)
                {
                    GradeAddedUnder50(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException("Enter grade between 0 and 100!");
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            foreach (var grade in grades)
            {
                result.Add(grade);
            }
            return result;
        }

        public override void AddNewArtist(string firstName, string lastName)
        {
            CheckFormatNames(firstName, lastName);
            string artist = $"{PascalCaseFormat(firstName)} {PascalCaseFormat(lastName)}";

            if (cast.Contains(artist))
            {
                throw new ArgumentException("Artist exist in cast!");
            }
            else
            {
                cast.Add(artist);
                if(NewArtistAdded != null)
                {
                    NewArtistAdded(this, new EventArgs());
                }
            }
        }

        public override void RemoveArtist(string firstName, string lastName)
        {
            if(cast.Count == 0)
            {
                throw new IndexOutOfRangeException("Cast is empty! Add at least one artist!");
            }

            CheckFormatNames(firstName, lastName);
            string artist = $"{PascalCaseFormat(firstName)} {PascalCaseFormat(lastName)}";

            if (!cast.Contains(artist))
            {
                throw new ArgumentException("Artist not exist in cast!");
            }
            else
            {
                this.cast.Remove(artist);
                if (ArtistRemoved != null)
                {
                    ArtistRemoved(this, new EventArgs());
                }
            }
        }

        public override void ShowCastOrMovieDetails(string input)
        {
            if (cast.Count == 0)
            {
                throw new IndexOutOfRangeException("IN MEMORY : Cast is empty!");
            }

            if (input == "Z")
            {
                Console.WriteLine($"IN MEMORY : Details of movie '{Title}'");
                Console.WriteLine($"\tTitle: {Title}");
                Console.WriteLine($"\tDirector: {Director}");
            }

            var index = 1;
            Console.WriteLine($"IN MEMORY : Full cast of movie '{Title}':");
            foreach (var artist in cast)
            {
                Console.WriteLine($"\t{index++} : {artist}");
            }
        }

        public bool IsEmptyCast()
        {
            if(cast.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}