namespace Assessment.Repositories
{
    using Assessment.Data;
    using Assessment.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Repository for customer interations to database.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext customerDbContext;
        private readonly ILogger<ICustomerRepository> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="customerDbContext">Customer DB Context.</param>
        /// <param name="logger">CustomerRepository logger.</param>
        public CustomerRepository(CustomerDbContext customerDbContext, ILogger<ICustomerRepository> logger)
        {
            this.customerDbContext = customerDbContext;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Customer?> GetCustomer(int id)
        {
            try
            {
                return await this.customerDbContext.Customer.FirstOrDefaultAsync(cust => cust.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving customer data; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Customer> AddCustomer(Customer customer)
        {
            try
            {
                this.customerDbContext.Customer.Add(customer);
                await this.customerDbContext.AddAsync(customer);
                this.customerDbContext.SaveChanges();

                return customer;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error saving customer to Customer database context; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UniqueCustomer(string firstname, string lastname)
        {
            try
            {
                var result = await this.customerDbContext.Customer.FirstOrDefaultAsync(cust => cust.FirstName == firstname && cust.LastName == lastname);

                return result == null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking customer name in Customer database context; {exception_message}", ex.Message);

                throw;
            }
        }
    }
}
