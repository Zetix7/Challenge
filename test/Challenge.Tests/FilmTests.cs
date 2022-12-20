using ChallengeApp;
using Xunit;

namespace Challenge.Tests
{
    public class FilmTests
    {
        [Fact]
        public void UseAddGradeMethodAndCheckResultsInGetStatistics()
        {
            // arrange
            var film = new InMemoryFilm("Star Wars");
            film.AddGrade(77.4);
            film.AddGrade(84.8);
            film.AddGrade(99.9);

            // act
            var result = film.GetStatistics();

            // assert
            Assert.Equal(3, result.Total);
            Assert.Equal(87.4, result.Average, 1);
            Assert.Equal(99.9, result.High);
            Assert.Equal(77.4, result.Low);
        }

        [Fact]
        public void AddGradesFromTableAndCheckResultsInGetStatistics()
        {
            // arrange
            var film = new InMemoryFilm("The Lion King");
            var grades = new double[] { 99, 92.2, 88.7, 77.7 };
            foreach (var g in grades)
                film.AddGrade(g);

            // act
            var result = film.GetStatistics();

            // assert
            Assert.Equal(4, result.Total);
            Assert.Equal(89.4, result.Average, 1);
            Assert.Equal(99, result.High);
            Assert.Equal(77.7, result.Low);
        }
    }
}