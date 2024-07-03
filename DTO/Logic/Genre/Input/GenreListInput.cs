using DTO.General.Pagination.Input;

namespace DTO.Logic.Genre.Input
{
    public class GenreListInput
    {
        public GenreListInput() { }

        public GenreFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
