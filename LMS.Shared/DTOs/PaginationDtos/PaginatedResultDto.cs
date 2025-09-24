using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.PaginationDtos
{
    /// <summary>
    /// Represents a paginated result containing a collection of items and associated pagination metadata.
    /// This DTO is used to provide clients with both the data for the current page and details about the overall pagination state.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated result.</typeparam>
    public class PaginatedResultDto<T>
    {
        /// <summary>
        /// Gets or sets the collection of items for the current page.
        /// This property contains the subset of data that corresponds to the requested page and page size.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with the paginated result.
        /// This includes details such as the total number of items, total pages, current page, and navigation flags.
        /// </summary>
        public PaginationMetadataDto Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResultDto{T}"/> class.
        /// </summary>
        /// <param name="items">The collection of items for the current page.</param>
        /// <param name="metadata">The metadata associated with the paginated result.</param>
        public PaginatedResultDto(IEnumerable<T> items, PaginationMetadataDto metadata)
        {
            Items = items;
            Metadata = metadata;
        }
    }
}
