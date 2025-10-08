using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.PaginationDtos
{
    /// <summary>
    /// DTO for paginated queries with optional filtering and sorting.
    /// </summary>
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DTO for paginated queries with optional filtering and sorting.
    /// </summary>
    public class PaginatedQueryDto
    {
        /// <summary>
        /// Page number to retrieve (default is 1). Must be greater than 0.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page (default is 10). Must be greater than 0.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Field to sort by (e.g., "Name", "CreatedAt").
        /// Optional, but if specified, must not be empty.
        /// </summary>
        [StringLength(100, ErrorMessage = "SortBy cannot exceed 100 characters.")]
        public string? SortBy { get; set; }

        /// <summary>
        /// Sort direction: "asc" or "desc". Default is ascending.
        /// </summary>
        [RegularExpression("asc|desc", ErrorMessage = "SortDirection must be 'asc' or 'desc'.")]
        public string SortDirection { get; set; } = "asc";

        /// <summary>
        /// Optional filter value to search by a specific field (e.g., name contains).
        /// </summary>
        [StringLength(200, ErrorMessage = "Filter value cannot exceed 200 characters.")]
        public string? Filter { get; set; }

        /// <summary>
        /// Field to apply the filter to (e.g., "Name", "Title").
        /// </summary>
        [StringLength(100, ErrorMessage = "FilterBy cannot exceed 100 characters.")]
        public string? FilterBy { get; set; }

        /// <summary>
        /// Optional related entities to include (comma-separated, e.g., "Documents,Activities").
        /// </summary>
        public string? Include { get; set; }
    }

}
