namespace Assessment.Tests.Repositories
{
    using Assessment.Data;
    using Assessment.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class CustomerRepositoryTest : IClassFixture<CustomerRepositoryTestFixture>
    {
        private CustomerRepositoryTestFixture fixture { get; }

        public CustomerRepositoryTest(CustomerRepositoryTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetCustomers_Success()
        {
            // Arrange
            // Use special database to not clash with other tests
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: "GetCustomersTest1")
                .Options;
            using var context = new CustomerDbContext(dbContext);
            var repository = this.fixture.Create(context);

            // Add customer 1
            var customer1 = CreateValidCustomer(2);
            await repository.AddCustomer(customer1);

            // Add customer 2
            var customer2 = CreateValidCustomer(3);
            await repository.AddCustomer(customer2);

            // Act
            var result = await repository.GetCustomers();

            // Assert
            Assert.Equal(2, result?.Count);
            Assert.Equal(customer1.Id, result?.First().Id);
            Assert.Equal(customer1.FirstName, result?.First().FirstName);
            Assert.Equal(customer1.LastName, result?.First().LastName);
            Assert.Equal(customer1.Address, result?.First().Address);
            Assert.Equal(customer1.Created, result?.First().Created);
            Assert.Equal(customer2.Id, result?.Last().Id);
        }

        [Fact]
        public async Task GetCustomers_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using var context = new CustomerDbContext(dbContext);
            var repository = this.fixture.Create(context);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetCustomers());

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains("Error retrieving all customers; No database provider has been configured for this DbContext.", LogLevel.Error);
        }

        [Fact]
        public async Task AddCustomer_Success()
        {
            // Arrange
            using var context = new CustomerDbContext(this.fixture.DbContext);
            var repository = this.fixture.Create(context);
            var customer = CreateValidCustomer(7);

            // Act
            await repository.AddCustomer(customer);

            var dbCustomer = context.Customer.FirstOrDefault(_ => _.Id == customer.Id);

            // Assert
            Assert.Equal(customer.Id, dbCustomer?.Id);
            Assert.Equal(customer.FirstName, dbCustomer?.FirstName);
            Assert.Equal(customer.LastName, dbCustomer?.LastName);
            Assert.Equal(customer.Address, dbCustomer?.Address);
            Assert.Equal(customer.Created, dbCustomer?.Created);
        }

        [Fact]
        public async Task AddCustomer_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using var context = new CustomerDbContext(dbContext);
            var repository = this.fixture.Create(context);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddCustomer(new Customer()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains("Error saving customer to Customer database context; No database provider has been configured for this DbContext.", LogLevel.Error);
        }

        [Fact]
        public async Task UpdateCustomer_Success()
        {
            // Arrange
            using var context = new CustomerDbContext(this.fixture.DbContext);
            var repository = this.fixture.Create(context);
            var customer = CreateValidCustomer(8);

            await repository.AddCustomer(customer);

            // Update customer values
            customer.FirstName = "UpdatedFirst";
            customer.LastName = "UpdatedLast";
            customer.Address = "UpdatedAddress";

            // Act
            await repository.UpdateCustomer(customer);

            var dbCustomer = context.Customer.FirstOrDefault(_ => _.Id == customer.Id);

            // Assert
            Assert.Equal(customer.Id, dbCustomer?.Id);
            Assert.Equal(customer.FirstName, dbCustomer?.FirstName);
            Assert.Equal(customer.LastName, dbCustomer?.LastName);
            Assert.Equal(customer.Address, dbCustomer?.Address);
            Assert.Equal(customer.Created, dbCustomer?.Created);
        }

        [Fact]
        public async Task UpdateCustomer_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using var context = new CustomerDbContext(dbContext);
            var repository = this.fixture.Create(context);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateCustomer(new Customer()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains("Error updating customer to Customer database context; No database provider has been configured for this DbContext.", LogLevel.Error);
        }

        private Customer CreateValidCustomer(int customerId)
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
