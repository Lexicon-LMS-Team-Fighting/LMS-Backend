namespace LMS.Shared.Pagination
{
    /// <summary>
    /// Represents a paginated result containing a collection of items and associated pagination metadata.
    /// This class is used to encapsulate the data for a specific page along with details about the overall pagination state.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated result.</typeparam>
    public class PaginatedResult<T> : IPaginatedResult<T>
    {
        /// <summary>
        /// Gets the collection of items for the current page.
        /// This property contains the subset of data that corresponds to the requested page and page size.
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets the metadata associated with the paginated result.
        /// This includes details such as the total number of items, total pages, current page, and navigation flags.
        /// </summary>
        public IPaginationMetadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
        /// </summary>
        /// <param name="items">The collection of items for the current page.</param>
        /// <param name="metadata">The metadata associated with the paginated result.</param>
        public PaginatedResult(IEnumerable<T> items, IPaginationMetadata metadata)
        {
            Items = items;
            Metadata = metadata;
        }
    }
}