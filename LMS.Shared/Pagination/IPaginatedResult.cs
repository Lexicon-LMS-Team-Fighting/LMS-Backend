using System.Collections.Generic;

namespace LMS.Shared.Pagination
{
    /// <summary>
    /// Defines the contract for a paginated result.
    /// This interface specifies the structure for encapsulating a collection of items
    /// along with metadata that describes the pagination state.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated result.</typeparam>
    public interface IPaginatedResult<T>
    {
        /// <summary>
        /// Gets the collection of items for the current page.
        /// This property contains the subset of data that corresponds to the requested page and page size.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets the metadata associated with the paginated result.
        /// This includes details such as the total number of items, total pages, current page, and navigation flags.
        /// </summary>
        IPaginationMetadata Metadata { get; }
    }
}