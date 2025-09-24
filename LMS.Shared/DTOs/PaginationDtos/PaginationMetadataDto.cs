namespace LMS.Shared.DTOs.PaginationDtos
{
    /// <summary>
    /// Represents metadata information for paginated results.
    /// This DTO is used to provide clients with details about the pagination state,
    /// such as the total number of items, total pages, current page, and navigation flags.
    /// </summary>
    public class PaginationMetadataDto
    {
        /// <summary>
        /// Gets or sets the total number of items in the data source.
        /// This value represents the total count of all items that match the query criteria,
        /// regardless of the current page or page size.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages available.
        /// This value is calculated based on the <see cref="TotalItems"/> and <see cref="PageSize"/>.
        /// It indicates how many pages of data are available for the given page size.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// This value indicates the page of data currently being viewed or requested.
        /// The page number is typically 1-based, meaning the first page is page 1.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// This value determines how many items are included in each page of the paginated result.
        /// It is used in conjunction with <see cref="CurrentPage"/> to calculate the range of items displayed.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a previous page available.
        /// This flag is useful for clients to determine if they can navigate to a page before the current one.
        /// It is <c>true</c> if <see cref="CurrentPage"/> is greater than 1; otherwise, <c>false</c>.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page available.
        /// This flag is useful for clients to determine if they can navigate to a page after the current one.
        /// It is <c>true</c> if <see cref="CurrentPage"/> is less than <see cref="TotalPages"/>; otherwise, <c>false</c>.
        /// </summary>
        public bool HasNextPage { get; set; }
    }
}
