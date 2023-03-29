namespace Assessment.Cache
{
    using Assessment.Models;

    /// <summary>
    /// Interface for Customer Cache Manager.
    /// </summary>
    public interface ICustomerCacheManager
    {
        /// <summary>
        /// Get all existing customers.
        /// </summary>
        /// <returns>Task of List of Customers.</returns>
        Task<List<Customer>?> GetCustomers();

        /// <summary>
        /// Get existing customer by id.
        /// </summary>
        /// <param name="id">Id of customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer?> GetCustomer(int id);

        /// <summary>
        /// Add Customer to Customer cache and database.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> AddCustomer(Customer customer);

        /// <summary>
        /// Check if this is a unique customer name.
        /// </summary>
        /// <param name="firstname">Customer firstname.</param>
        /// <param name="lastname">Customer lastname.</param>
        /// <returns>Task of bool.</returns>
        Task<bool> UniqueCustomer(string firstname, string lastname);
    }
}
