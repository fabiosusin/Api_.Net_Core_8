using DTO.General.Base.Database;
using DTO.Logic.Movie.Input;
using System;
using System.Collections.Generic;

namespace DTO.Logic.Movie.Database
{
    public class Movie : BaseData
    {
        public Movie() { }
        public Movie(MovieInput movie)
        {
            if (movie == null)
                return;

            Title = movie.Title;
            GenreId = movie.GenreId;
            ReleaseDate = movie.ReleaseDate;
            StreamingsId = movie.StreamingsId;
        }

        public string Title { get; set; }
        public string GenreId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> StreamingsId { get; set; }
    }
}
