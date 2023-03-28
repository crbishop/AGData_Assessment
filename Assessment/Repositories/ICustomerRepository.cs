namespace Assessment.Repositories
{
    using Assessment.Models;

    /// <summary>
    /// Interface for Customer Repository.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Get existing customer.
        /// </summary>
        /// <param name="id">Id of customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer?> GetCustomer(int id);

        /// <summary>
        /// Add Customer to Customer database.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> AddCustomer(Customer customer);
    }
}
