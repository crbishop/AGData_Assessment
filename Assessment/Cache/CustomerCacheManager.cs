namespace Assessment.Cache
{
    using Assessment.Models;
    using Assessment.Repositories;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Customer cache manager.
    /// </summary>
    public class CustomerCacheManager : ICustomerCacheManager
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<ICustomerCacheManager> logger;
        private readonly IMemoryCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerCacheManager"/> class.
        /// </summary>
        /// <param name="customerRepository">Customer repository.</param>
        /// <param name="logger">CustomerCacheManager logger.</param>
        /// <param name="cache">Memory cache.</param>
        public CustomerCacheManager(ICustomerRepository customerRepository, ILogger<ICustomerCacheManager> logger, IMemoryCache cache)
        {
            this.customerRepository = customerRepository;
            this.logger = logger;
            this.cache = cache;
        }

        /// <inheritdoc/>
        public async Task<List<Customer>?> GetCustomers()
        {
            try
            {
                if (!this.cache.TryGetValue(CacheKeys.Customers, out List<Customer>? customers))
                {
                    customers = await this.customerRepository.GetCustomers();

                    this.cache.Set(CacheKeys.Customers, customers, CacheOptions());

                    this.logger.LogInformation("Memory cache created.");
                }
                else
                {
                    this.logger.LogInformation("Memory cache used.");
                }

                return customers;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving all cached customers; {exception_message}", ex.Message);

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Customer?> GetCustomer(int id)
        {
            Customer? customer;

            try
            {
                // Retrieve customer cache
                var customers = await this.CheckCache();

                customer = customers?.FirstOrDefault(cust => cust.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving a cached customer by Id; {exception_message}", ex.Message);

                throw;
            }

            return customer;
        }

        /// <inheritdoc/>
        public async Task<Customer> AddCustomer(Customer customer)
        {
            var newCustomer = await this.customerRepository.AddCustomer(customer);

            try
            {
                // Retrieve customer cache
                // Get customer cache and either establish it if null
                var custCache = this.cache.Get<List<Customer>?>(CacheKeys.Customers);

                if (custCache == null)
                {
                    await this.GetCustomers();
                }
                else
                {
                    custCache?.Add(newCustomer);
                    this.cache.Set(CacheKeys.Customers, custCache);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error saving customer to Customer cache; {exception_message}", ex.Message);

                throw;
            }

            return newCustomer;
        }

        /// <inheritdoc/>
        public async Task<bool> UniqueCustomer(string firstname, string lastname)
        {
            try
            {
                // Retrieve customer cache
                var customers = await this.CheckCache();

                var result = customers?.FirstOrDefault(cust => cust.FirstName == firstname && cust.LastName == lastname);

                return result == null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking customer name in Customer cache; {exception_message}", ex.Message);

                throw;
            }
        }

        // Cache options.
        private static MemoryCacheEntryOptions CacheOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 256,
            };
        }

        // Check existing cache and establish/reset if necessary
        private async Task<List<Customer>?> CheckCache()
        {
            // Get customer cache and either establish it if null
            var custCache = this.cache.Get<List<Customer>?>(CacheKeys.Customers);

            if (custCache == null)
            {
                custCache = await this.GetCustomers();
            }

            return custCache;
        }
    }
}
