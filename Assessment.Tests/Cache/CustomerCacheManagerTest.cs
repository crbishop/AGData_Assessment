namespace Assessment.Tests.Cache
{
    using Assessment.Cache;
    using Assessment.Models;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class CustomerCacheManagerTest : IClassFixture<CustomerCacheManagerTestFixture>
    {
        private CustomerCacheManagerTestFixture fixture { get; }

        public CustomerCacheManagerTest(CustomerCacheManagerTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetCustomers_Success()
        {
            // Arrange
            var customer1 = CreateValidCustomer(1);
            var customer2 = CreateValidCustomer(2);
            var customerList = new List<Customer>
            {
                customer1,
                customer2
            };

            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Act
            var result = await cacheManager.GetCustomers();

            // Assert
            Assert.Equal(2, result?.Count);
            Assert.Equal(customer1.Id, result?.First().Id);
            Assert.Equal(customer1.FirstName, result?.First().FirstName);
            Assert.Equal(customer1.LastName, result?.First().LastName);
            Assert.Equal(customer1.Address, result?.First().Address);
            Assert.Equal(customer1.Created, result?.First().Created);
            Assert.Equal(customer2.Id, result?.Last().Id);

            // Get cache count
            var custCache = this.fixture.cache.Get<List<Customer>?>(CacheKeys.Customers);
            Assert.Equal(2, custCache?.Count);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task GetCustomers_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);

            var cacheManager = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => cacheManager.GetCustomers());

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains($"Error retrieving all cached customers", LogLevel.Error);
        }

        [Fact]
        public async Task GetCustomerById_Success()
        {
            // Arrange
            var customerId = 1;
            var customer = CreateValidCustomer(customerId);
            var customerList = new List<Customer>
            {
                customer,
            };

            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Act
            var result = await cacheManager.GetCustomer(customerId);

            // Assert
            Assert.Same(customer, result);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task GetCustomerById_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);

            var cacheManager = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => cacheManager.GetCustomer(1));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains($"Error retrieving a cached customer by Id", LogLevel.Error);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task AddCustomer_Success()
        {
            // Arrange
            var customer1 = CreateValidCustomer(1);
            var customer2 = CreateValidCustomer(2);

            // Only add customer1
            var customerList = new List<Customer>
            {
                customer1,
            };

            this.fixture.SetupAddCustomer(customer2);
            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Setup cache
            await cacheManager.GetCustomers();

            // Act
            var result = await cacheManager.AddCustomer(customer2);
            Assert.Same(customer2, result);

            // Second customer item was added
            var custCache = this.fixture.cache.Get<List<Customer>?>(CacheKeys.Customers);
            Assert.Equal(2, custCache?.Count);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task AddCustomer_ThrowsException()
        {
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);
            this.fixture.SetupAddCustomer(new Customer());

            var cacheManager = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => cacheManager.AddCustomer(new Customer()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains($"Error saving customer to Customer cache", LogLevel.Error);
        }

        [Fact]
        public async Task UpdateCustomer_Success()
        {
            // Arrange
            var customer1 = CreateValidCustomer(1);
            var customer2 = new Customer
            {
                Id = 2,
                FirstName = "BadFirst",
                LastName = "BadLast",
                Address = "BadAddress",
                Created = DateTime.Parse("03/29/2023 01:02:03"),
            };

            var customerList = new List<Customer>
            {
                customer1,
                customer2,
            };

            var updatedCustomer = new Customer
            {
                Id = customer2.Id,
                FirstName = "GoodFirst",
                LastName = "GoodLast",
                Address = "GoodAddress",
                Created = DateTime.Parse("03/29/2023 01:02:03"),
            };

            this.fixture.SetupUpdateCustomer(updatedCustomer);
            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Setup cache
            await cacheManager.GetCustomers();

            // Act
            var result = await cacheManager.UpdateCustomer(updatedCustomer);
            Assert.Same(updatedCustomer, result);

            // Second customer item was updated
            var custCache = this.fixture.cache.Get<List<Customer>?>(CacheKeys.Customers);
            var custUpdated = custCache?.FirstOrDefault(cust => cust.Id == updatedCustomer.Id);
            Assert.Equal(updatedCustomer.FirstName, custUpdated?.FirstName);
            Assert.Equal(updatedCustomer.LastName, custUpdated?.LastName);
            Assert.Equal(updatedCustomer.Address, custUpdated?.Address);
            Assert.Equal(customer2.Created, custUpdated?.Created);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task UpdateCustomer_ThrowsException()
        {
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);
            this.fixture.SetupUpdateCustomer(new Customer());

            var cacheManager = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => cacheManager.UpdateCustomer(new Customer()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains($"Error updating customer to Customer cache", LogLevel.Error);
        }

        [Fact]
        public async Task UniqueCustomer_Success()
        {
            // Arrange
            var customer = CreateValidCustomer(1);
            customer.FirstName = "Joe";
            customer.LastName = "Camel";
            var customerList = new List<Customer>
            {
                customer,
            };

            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Act
            var result = await cacheManager.UniqueCustomer(customer.FirstName, customer.LastName);

            // Assert
            Assert.False(result);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task UniqueCustomer_UniqueFirstName()
        {
            // Arrange
            var customer = CreateValidCustomer(1);
            customer.FirstName = "Joe";
            customer.LastName = "Camel";
            var customerList = new List<Customer>
            {
                customer,
            };

            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Act
            var result = await cacheManager.UniqueCustomer("Unique", customer.LastName);

            // Assert
            Assert.True(result);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task UniqueCustomer_UniqueLastName()
        {
            // Arrange
            var customer = CreateValidCustomer(1);
            customer.FirstName = "Joe";
            customer.LastName = "Camel";
            var customerList = new List<Customer>
            {
                customer,
            };

            this.fixture.SetupGetCustomers(customerList);

            var cacheManager = this.fixture.Create();

            // Act
            var result = await cacheManager.UniqueCustomer(customer.FirstName, "Unique");

            // Assert
            Assert.True(result);

            // Cleanup
            this.fixture.CleanupCache();
        }

        [Fact]
        public async Task UniqueCustomer_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);

            var cacheManager = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => cacheManager.UniqueCustomer("A", "Name"));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains($"Error checking customer name in Customer cache", LogLevel.Error);
        }

        private static Customer CreateValidCustomer(int customerId)
        {
            return new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Test Ave",
                Created = DateTime.Parse("03/27/2023 01:02:03")
            };
        }
    }
}
