using DTO.General.Pagination.Input;

namespace DTO.Logic.Rating.Input
{
    public class RatingListInput
    {
        public RatingListInput() { }
        public RatingFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
