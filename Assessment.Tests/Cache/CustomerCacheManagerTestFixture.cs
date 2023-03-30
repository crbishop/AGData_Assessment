namespace Assessment.Tests.Cache
{
    using Assessment.Cache;
    using Assessment.Models;
    using Assessment.Repositories;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomerCacheManagerTestFixture
    {
        public Mock<ICustomerRepository> customerRepositoryMock;
        public Mock<ILogger<ICustomerCacheManager>> LoggerMock { get; }
        public IMemoryCache cache;

        public CustomerCacheManagerTestFixture()
        {
            this.customerRepositoryMock = new Mock<ICustomerRepository>();
            this.LoggerMock = new Mock<ILogger<ICustomerCacheManager>>();
            this.cache = new MemoryCache(new MemoryCacheOptions());
        }

        public CustomerCacheManager Create()
        {
            return new CustomerCacheManager(this.customerRepositoryMock.Object, this.LoggerMock.Object, this.cache);
        }

        public void SetupGetCustomers(List<Customer>? expectedCustomers)
        {
            this.customerRepositoryMock.Setup(_ => _.GetCustomers()).ReturnsAsync(expectedCustomers);
        }

        public void SetupAddCustomer(Customer createdCustomer)
        {
            this.customerRepositoryMock.Setup(_ => _.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(createdCustomer);
        }

        public void SetupUpdateCustomer(Customer updatedCustomer)
        {
            this.customerRepositoryMock.Setup(_ => _.UpdateCustomer(It.IsAny<Customer>())).ReturnsAsync(updatedCustomer);
        }

        public void SetupDeleteCustomer()
        {
            this.customerRepositoryMock.Setup(_ => _.DeleteCustomer(It.IsAny<Customer>()));
        }

        public void SetupGetCustomersThrowsException(string exceptionMessage)
        {
            this.customerRepositoryMock.Setup(_ => _.GetCustomers()).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void CleanupCache()
        {
            cache.Remove(CacheKeys.Customers);
        }
    }
}
