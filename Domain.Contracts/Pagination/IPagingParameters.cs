namespace Domain.Contracts.Pagination
{
    /// <summary>
    /// Defines the contract for pagination parameters.
    /// This interface specifies the essential properties required for implementing pagination,
    /// such as the current page number and the number of items per page.
    /// </summary>
    public interface IPagingParameters
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// This value determines which page of data is being requested.
        /// The page number is typically 1-based, meaning the first page is page 1.
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// This value determines how many items are included in each page of the paginated result.
        /// It is used in conjunction with <see cref="PageNumber"/> to calculate the range of items displayed.
        /// </summary>
        int PageSize { get; set; }
    }
}