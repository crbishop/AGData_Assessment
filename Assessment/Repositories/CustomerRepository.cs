﻿namespace Assessment.Repositories
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
        public async Task<List<Customer>?> GetCustomers()
        {
            try
            {
                return await this.customerDbContext.Customer.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving all customers; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Customer> AddCustomer(Customer customer)
        {
            try
            {
                this.customerDbContext.Add(customer);
                await this.customerDbContext.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error saving customer to Customer database; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            try
            {
                this.customerDbContext.Update(customer);
                await this.customerDbContext.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating customer to Customer database; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteCustomer(Customer customer)
        {
            try
            {
                this.customerDbContext.Remove(customer);
                await this.customerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting customer from Customer database; {exception_message}", ex.Message);

                throw;
            }
        }
    }
}
