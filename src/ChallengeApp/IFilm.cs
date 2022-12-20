using System;

namespace ChallengeApp
{
    public interface IFilm
    {
        string Title { get; set; }
        string Director { get; set; }

        void AddGrade(double grade);
        void AddGrade(string grade);
        void AddGrade(char grade);
        Statistics GetStatistics();
        void AddNewArtist(string firstName, string lastName);
        void RemoveArtist(string firstName, string lastName);
        void ShowCastOrMovieDetails(string input);
    }
}