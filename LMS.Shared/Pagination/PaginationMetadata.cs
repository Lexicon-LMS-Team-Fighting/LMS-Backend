namespace LMS.Shared.Pagination
{
    /// <summary>
    /// Represents metadata information for a paginated result.
    /// This class provides details about the pagination state, such as the total number of items,
    /// total pages, current page, and navigation flags.
    /// </summary>
    public class PaginationMetadata : IPaginationMetadata
    {
        /// <summary>
        /// Gets the total number of items in the data source.
        /// This value represents the total count of all items that match the query criteria,
        /// regardless of the current page or page size.
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Gets the total number of pages available.
        /// This value is calculated based on the <see cref="TotalItems"/> and <see cref="PageSize"/>.
        /// It indicates how many pages of data are available for the given page size.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the current page number.
        /// This value indicates the page of data currently being viewed or requested.
        /// The page number is typically 1-based, meaning the first page is page 1.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Gets the number of items per page.
        /// This value determines how many items are included in each page of the paginated result.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page available.
        /// This flag is useful for clients to determine if they can navigate to a page before the current one.
        /// It is <c>true</c> if <see cref="CurrentPage"/> is greater than 1; otherwise, <c>false</c>.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page available.
        /// This flag is useful for clients to determine if they can navigate to a page after the current one.
        /// It is <c>true</c> if <see cref="CurrentPage"/> is less than <see cref="TotalPages"/>; otherwise, <c>false</c>.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationMetadata"/> class.
        /// </summary>
        /// <param name="totalItems">The total number of items in the data source.</param>
        /// <param name="currentPage">The current page number being viewed or requested.</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PaginationMetadata(int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}