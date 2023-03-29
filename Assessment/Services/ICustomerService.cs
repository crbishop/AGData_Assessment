namespace Assessment.Services
{
    using Assessment.Models;

    /// <summary>
    /// Interface for Customer Service.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Add new Customer.
        /// </summary>
        /// <param name="customer">Customer Input.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> AddCustomer(CustomerInput customer);

        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <param name="customerInput">Customer Input.</param>
        /// <param name="customer">Existing Customer.</param>
        /// <returns>Task of Customer.</returns>
        Task<Customer> UpdateCustomer(CustomerInput customerInput, Customer customer);
    }
}
