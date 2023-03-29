namespace Assessment.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Customer input data.
    /// </summary>
    public class CustomerInput
    {
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}
