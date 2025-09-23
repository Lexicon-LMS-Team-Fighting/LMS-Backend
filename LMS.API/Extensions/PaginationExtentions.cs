using Domain.Contracts.Pagination;
using Domain.Models.Pagination;

namespace LMS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for applying pagination logic to data sources.
    /// This class contains reusable functionality to simplify the process of paginating data
    /// and generating metadata for the paginated result.
    /// </summary>
    public static class PaginationExtensions
    {
        /// <summary>
        /// Converts an <see cref="IQueryable{T}"/> data source into a paginated result.
        /// This method applies pagination logic (e.g., skipping and taking items) and generates
        /// metadata about the pagination state.
        /// </summary>
        /// <typeparam name="T">The type of items in the data source.</typeparam>
        /// <param name="source">The data source to paginate, represented as an <see cref="IQueryable{T}"/>.</param>
        /// <param name="pagingParameters">The pagination parameters, including the page number and page size.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{T}"/> containing the items for the current page and metadata
        /// about the pagination state (e.g., total items, total pages, current page, etc.).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="source"/> or <paramref name="pagingParameters"/> is null.
        /// </exception>
        public static PaginatedResult<T> ToPaginatedResult<T>(this IQueryable<T> source, IPagingParameters pagingParameters)
        {
            var totalItems = source.Count();

            // Retrieve the items for the current page
            var items = source
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToList();

            // Generate pagination metadata
            var metadata = new PaginationMetadata(totalItems, pagingParameters.PageNumber, pagingParameters.PageSize);

            // Return the paginated result containing the items and metadata
            return new PaginatedResult<T>(items, metadata);
        }
    }
}
