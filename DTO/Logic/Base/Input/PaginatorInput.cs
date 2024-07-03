namespace DTO.General.Pagination.Input
{
    public class PaginatorInput
    {
        public PaginatorInput() { }
        public PaginatorInput(int page, int resultsPerPage)
        {
            Page = page;
            ResultsPerPage = resultsPerPage;
        }

        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string SortColumn { get; set; }
        public string SortType { get; set; }
    }
}
