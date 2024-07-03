using System;
using System.Collections.Generic;

namespace DTO.Logic.Movie.Input
{
    public class MovieInput
    {
        public string Title { get; set; }
        public string GenreId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> StreamingsId { get; set; }
    }
}
