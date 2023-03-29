namespace Assessment.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Customer database table.
    /// </summary>
    [Table("customer")]
    public class Customer
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        [Column("firstname")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        [Column("lastname")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the created datetime.
        /// </summary>
        [Column("created")]
        public DateTimeOffset? Created { get; set; }

        /// <summary>
        /// Gets or sets the updated datetime.
        /// </summary>
        [Column("updated")]
        public DateTimeOffset Updated { get; set; }
    }
}
