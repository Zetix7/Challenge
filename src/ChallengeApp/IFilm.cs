using System;

namespace ChallengeApp
{
    public interface IFilm
    {
        string Title { get; set; }

        void AddGrade(double grade);
        void AddGrade(string grade);
        void AddGrade(char grade);
        Statistics GetStatistics();
    }
}