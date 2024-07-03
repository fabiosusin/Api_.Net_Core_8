namespace DTO.Logic.Movie.Input
{
    public class MovieFiltersInput
    {
        public MovieFiltersInput() { }
        public MovieFiltersInput(string title) => Title = title;
        public string Title { get; set; }
        public string GenreId { get; set; }
    }
}