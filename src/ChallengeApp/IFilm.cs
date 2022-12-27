using System;

namespace ChallengeApp
{
    public interface IFilm
    {
        string Title { get; set; }

        delegate void GradeAddedDelegate(object sender, EventArgs arg);

        event GradeAddedDelegate GradeAdded;
        event GradeAddedDelegate GradeAddedUnder50;

        void AddGrade(double grade);
        void AddGrade(string grade);
        void AddGrade(char grade);
        Statistics GetStatistics();
    }
}