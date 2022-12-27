using System;

namespace ChallengeApp
{
    public interface IFilm
    {
        string Title { get; set; }
        string Director { get; set; }

        delegate void GradeAddedDelegate(object sender, EventArgs arg);
        delegate void NewArtistAddedDelegate(object sender, EventArgs arg);
        delegate void ArtistRemovedDelegate(object sender, EventArgs arg);

        event GradeAddedDelegate GradeAdded;
        event GradeAddedDelegate GradeAddedUnder50;
        event NewArtistAddedDelegate NewArtistAdded;
        event ArtistRemovedDelegate ArtistRemoved;

        void AddGrade(double grade);
        void AddGrade(string grade);
        void AddGrade(char grade);
        Statistics GetStatistics();
        void AddNewArtist(string firstName, string lastName);
        void RemoveArtist(string firstName, string lastName);
        void ShowCastOrMovieDetails(string input);
    }
}