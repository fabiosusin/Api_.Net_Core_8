using DTO.General.Pagination.Input;

namespace DTO.Logic.Streaming.Input
{
    public class StreamingListInput
    {
        public StreamingListInput() { }

        public StreamingFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
