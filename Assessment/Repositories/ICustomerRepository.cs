namespace Assessment.Repositories
{
    using Assessment.Models;

    /// <summary>
    /// Interface for Customer Repository.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Get all existing customers.
        /// </summary>
        /// <returns>Task of List of Customers.</returns>
        Task<List<Customer>?> GetCustomers();

        /// <summary>
        /// Add Customer to Customer database.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> AddCustomer(Customer customer);

        /// <summary>
        /// Update Customer in Customer database.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> UpdateCustomer(Customer customer);

        /// <summary>
        /// Delete Customer from Customer database.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Task.</returns>
        Task DeleteCustomer(Customer customer);
    }
}
