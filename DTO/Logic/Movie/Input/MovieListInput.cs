using DTO.General.Pagination.Input;

namespace DTO.Logic.Movie.Input
{
    public class MovieListInput
    {
        public MovieListInput() { }
        public MovieListInput(string name) => Filters = new(name);
        public MovieFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
