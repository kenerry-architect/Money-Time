using System;
using System.Collections.Generic;

namespace MoneyTime.Domain.Core.Queries
{
    public class PaginatedResults<TEntity> where TEntity : class
    {
        public PaginatedResults
        (
            int currentPage,
            int totalRecords,
            int recordsPerPage,
            ICollection<TEntity> results
        )
        {
            CurrentPage = currentPage;
            TotalRecords = totalRecords;
            RecordsPerPage = recordsPerPage;
            Results = results;
            TotalPages = (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
        }

        public int TotalPages { get; }
        public int CurrentPage { get; }
        public int TotalRecords { get; }
        public int RecordsPerPage { get; }
        public ICollection<TEntity> Results { get; }
    }
}
