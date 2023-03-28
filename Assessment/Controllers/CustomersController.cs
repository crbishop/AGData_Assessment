namespace AgData_Assessment.Controllers
{
    using System.Net;
    using Assessment.Models;
    using Assessment.Repositories;
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
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<CustomersController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerService">Customer Service.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="logger">Logger instance.</param>
        public CustomersController(ICustomerService customerService, ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gets exising customer.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <returns>Customer.</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = await this.customerRepository.GetCustomer(id);

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
            if (customerInput == null)
            {
                return this.BadRequest();
            }

            try
            {
                var customer = await this.customerService.AddCustomer(customerInput);

                return this.CreatedAtAction(nameof(this.GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating a new customer; {exception_message}", ex.Message);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Error creating a new customer.");
            }
        }
    }
}