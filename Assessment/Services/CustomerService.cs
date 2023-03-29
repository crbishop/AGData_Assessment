namespace Assessment.Services
{
    using Assessment.Cache;
    using Assessment.Models;
    using AutoMapper;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerCacheManager customerCacheManager;
        private readonly IMapper mapper;
        private readonly ILogger<ICustomerService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="customerCacheManager">Customer cache manager.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="logger">CustomerService logger.</param>
        public CustomerService(
            ICustomerCacheManager customerCacheManager,
            IMapper mapper,
            ILogger<ICustomerService> logger)
        {
            this.customerCacheManager = customerCacheManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Customer> AddCustomer(CustomerInput customerInput)
        {
            Customer customer;

            try
            {
                // Map CustomerInput into a new Customer object.
                customer = this.mapper.Map<Customer>(customerInput);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error mapping customer input; {exception_message}", ex.Message);

                throw;
            }

            return await this.customerCacheManager.AddCustomer(customer);
        }
    }
}
