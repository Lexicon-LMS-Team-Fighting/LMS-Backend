using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.AuthDtos
{
    /// <summary>
    /// Data Transfer Object for creating a new user.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "FirstName must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "FirstName cannot exceed 20 characters.")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "LastName must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "LastName cannot exceed 20 characters.")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summry>
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Student|Teacher", ErrorMessage = "Role must be either 'Student' or 'Teacher'.")]
        public string Role { get; set; } = "Student";
    }
}
