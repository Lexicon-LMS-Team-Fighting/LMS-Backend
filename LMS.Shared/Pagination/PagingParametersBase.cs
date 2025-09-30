namespace LMS.Shared.Pagination
{
    /// <summary>
    /// Represents the base class for pagination parameters.
    /// This class provides the essential properties required for implementing pagination,
    /// such as the current page number and the number of items per page.
    /// </summary>
    public abstract class PagingParametersBase : IPagingParameters
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// This value determines which page of data is being requested.
        /// Defaults to 1, representing the first page.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// This value determines how many items are included in each page of the paginated result.
        /// Defaults to 20.
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}