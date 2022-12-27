using System;
using ChallengeApp;
using Xunit;

namespace Challenge.Tests
{
    public class TypeTests
    {
        public delegate string WriteMessage(string message);

        int counter = 0;

        [Fact]
        public void WriteMessageDelegateCanPointToMethod()
        {
            WriteMessage del = ReturnMessage;
            del += ReturnMessageToLower;

            var result = del("hi...!");

            Assert.Equal(2, counter);
        }


        [Fact]
        public void GetFilmReturnsDifferentObjects()
        {
            var film1 = GetFilm("Star Wars","George Lucas");
            var film2 = GetFilm("Avengers","Anthony Russo");

            Assert.Equal("Star Wars", film1.Title);
            Assert.Equal("Avengers", film2.Title);
            Assert.NotSame(film1, film2);
            Assert.False(Object.ReferenceEquals(film1, film2));
        }

        [Fact]
        public void TwoVarsCanReferenceSameObject()
        {
            var film1 = GetFilm("Star Wras","George Lucas");
            var film2 = film1;

            Assert.True(film1.Equals(film2));
            Assert.Same(film1, film2);
            Assert.True(Object.ReferenceEquals(film1, film2));
        }

        [Fact]
        public void CSharpCanPassByRef()
        {
            InMemoryFilm film = null;// GetFilm("Star Wars");
            GetFilmSetTitle(out film, "New Title", "New Director");
            Assert.Equal("New Title", film.Title);
        }

        [Fact]
        public void CanSetTitleFromReference()
        {
            var film1 = GetFilm("Starwars","George Lucas");
            this.SetTitle(film1, "New Title");
            Assert.Equal("New Title", film1.Title);
        }

        [Fact]
        public void SetIntByReference()
        {
            var x = GetInt();
            SetInt(ref x);

            Assert.Equal(7, x);
        }

        [Fact]
        public void StringsBehaveLikeValueType()
        {
            var x = "Star Wars";
            var upper = this.MakeUpperCase(x);

            Assert.Equal("Star Wars", x);
            Assert.Equal("STAR WARS", upper);
        }

        private string ReturnMessage(string message)
        {
            counter++;
            return message;
        }

        private string ReturnMessageToLower(string message)
        {
            counter++;
            return message.ToLower();
        }

        private void GetFilmSetTitle(out InMemoryFilm film, string title, string director)
        {
            film = GetFilm(title, director);
        }

        private InMemoryFilm GetFilm(string title, string director)
        {
            return new InMemoryFilm(title, director);
        }

        private void SetTitle(InMemoryFilm film, string title)
        {
            film.Title = title;
        }


        private int GetInt()
        {
            return 7;
        }

        private void SetInt(ref int x)
        {
            x = 7;
        }

        private string MakeUpperCase(string parametr)
        {
            return parametr.ToUpper();
        }
    }
}