namespace AgData_Assessment.Controllers
{
    using System.Net;
    using Assessment.Cache;
    using Assessment.Models;
    using Assessment.Services;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Customer Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerCacheManager customerCacheManager;
        private readonly ILogger<CustomersController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerService">Customer Service.</param>
        /// <param name="customerCacheManager">Customer Cache Manager.</param>
        /// <param name="logger">Logger instance.</param>
        public CustomersController(
            ICustomerService customerService,
            ICustomerCacheManager customerCacheManager,
            ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.customerCacheManager = customerCacheManager;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all existing customers.
        /// </summary>
        /// <returns>List of Customers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await this.customerCacheManager.GetCustomers();
                return this.Ok(customers);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving list of all customers; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error retrieving list of all customers.");
            }
        }

        /// <summary>
        /// Gets exising customer by Id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <returns>Customer.</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = await this.customerCacheManager.GetCustomer(id);

                if (customer == null)
                {
                    return this.NotFound();
                }

                return this.Ok(customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving customer data; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error retrieving customer data.");
            }
        }

        /// <summary>
        /// Create a new customer.
        /// </summary>
        /// <param name="customerInput">Customer Input.</param>
        /// <returns>Total number of Tweets.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerInput? customerInput)
        {
            try
            {
                var badRequestMessage = await this.ValidateCustomer(customerInput);
                if (!string.IsNullOrWhiteSpace(badRequestMessage))
                {
                    return this.BadRequest(badRequestMessage);
                }

#pragma warning disable CS8604 // Possible null reference argument.
                var customer = await this.customerService.AddCustomer(customerInput);
#pragma warning restore CS8604 // Possible null reference argument.

                return this.CreatedAtAction(nameof(this.GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating a new customer; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error creating a new customer.");
            }
        }

        /// <summary>
        /// Update a customer by Id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <param name="customerInput">Customer Input.</param>
        /// <returns>Customer.</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerInput? customerInput)
        {
            try
            {
                var badRequestMessage = await this.ValidateCustomer(customerInput);
                if (!string.IsNullOrWhiteSpace(badRequestMessage))
                {
                    return this.BadRequest(badRequestMessage);
                }

                var customer = await this.customerCacheManager.GetCustomer(id);

                if (customer == null)
                {
                    return this.NotFound();
                }

#pragma warning disable CS8604 // Possible null reference argument.
                customer = await this.customerService.UpdateCustomer(customerInput, customer);
#pragma warning restore CS8604 // Possible null reference argument.

                return this.Ok(customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating a customer; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error updating a customer.");
            }
        }

        /// <summary>
        /// Delete a customer by Id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <returns>No content.</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await this.customerCacheManager.GetCustomer(id);

                if (customer == null)
                {
                    return this.NotFound();
                }

                await this.customerCacheManager.DeleteCustomer(customer);

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting customer data; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error retrieving customer data.");
            }
        }

        private async Task<string> ValidateCustomer(CustomerInput? customerInput)
        {
            if (customerInput == null)
            {
                return "Customer input cannot be null.";
            }

            if (string.IsNullOrWhiteSpace(customerInput.FirstName) || string.IsNullOrWhiteSpace(customerInput.LastName))
            {
                return "Customer name cannot be null or empty.";
            }

            // Check if first and last name is unique
            if (!await this.customerCacheManager.UniqueCustomer(customerInput.FirstName, customerInput.LastName))
            {
                var customerName = customerInput.FirstName + " " + customerInput.LastName;
                return $"Customer first and last name ({customerName}) already exists.";
            }

            return string.Empty;
        }
    }
}