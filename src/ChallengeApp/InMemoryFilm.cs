using System;
using System.Collections.Generic;

namespace ChallengeApp
{
    public class InMemoryFilm : FilmBase
    {
        private List<double> grades = new();

        public InMemoryFilm(string title) : base(title) { }

        public event GradeAddedDelegate GradeAdded;
        public event GradeAddedDelegate GradeAddedUnder50;

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
    }
}