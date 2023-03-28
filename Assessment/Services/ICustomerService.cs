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
        /// <returns>Task.</returns>
        Task<Customer> AddCustomer(CustomerInput customer);
    }
}
