namespace MoneyTime.Domain.Core.Queries
{
    public sealed class PaginationInput
    {
        public PaginationInput(int currentPage, int recordsPerPage)
        {
            CurrentPage = currentPage;
            RecordsPerPage = recordsPerPage;
        }

        public int CurrentPage { get; }
        public int RecordsPerPage { get; }
        public int RecordsToSkip => (CurrentPage - 1) * RecordsPerPage;
    }
}
