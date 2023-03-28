namespace Assessment.Data
{
    using Assessment.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Customer database context.
    /// </summary>
    public class CustomerDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDbContext"/> class.
        /// </summary>
        /// <param name="options">Customer DbContext options.</param>
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Customer database entry.
        /// </summary>
        public DbSet<Customer> Customer { get; set; }
    }
}
