namespace Assessment.Tests.Repositories
{
    using Assessment.Data;
    using Assessment.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class CustomerRepositoryTest : IClassFixture<CustomerRepositoryTestFixture>
    {
        private CustomerRepositoryTestFixture Fixture { get; }

        public CustomerRepositoryTest(CustomerRepositoryTestFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task GetCustomer_Success()
        {
            // Arrange
            using (var context = new CustomerDbContext(this.Fixture.DbContext))
            {
                var repository = this.Fixture.Create(context);
                var customer = CreateValidCustomer(1);
                await repository.AddCustomer(customer);

                // Act
                var result = await repository.GetCustomer(customer.Id);

                // Assert
                Assert.Equal(customer.Id, result?.Id);
                Assert.Equal(customer.FirstName, result?.FirstName);
                Assert.Equal(customer.LastName, result?.LastName);
                Assert.Equal(customer.Address, result?.Address);
                Assert.Equal(customer.Created, result?.Created);
            }
        }

        [Fact]
        public async Task GetCustomer_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using (var context = new CustomerDbContext(dbContext))
            {
                var repository = this.Fixture.Create(context);

                // Act
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetCustomer(1));

                // Assert
                Assert.NotNull(exception);
                this.Fixture.LoggerMock.VerifyWasCalledContains("Error retrieving customer data; No database provider has been configured for this DbContext.", LogLevel.Error);
            }
        }

        [Fact]
        public async Task AddCustomer_Success()
        {
            // Arrange
            using (var context = new CustomerDbContext(this.Fixture.DbContext))
            {
                var repository = this.Fixture.Create(context);
                var customer = CreateValidCustomer(2);

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
        }

        [Fact]
        public async Task AddCustomer_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using (var context = new CustomerDbContext(dbContext))
            {
                var repository = this.Fixture.Create(context);

                // Act
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddCustomer(new Customer()));

                // Assert
                Assert.NotNull(exception);
                this.Fixture.LoggerMock.VerifyWasCalledContains("Error saving customer to Customer database context; No database provider has been configured for this DbContext.", LogLevel.Error);
            }
        }

        [Fact]
        public async Task UniqueCustomer_Success()
        {
            using (var context = new CustomerDbContext(this.Fixture.DbContext))
            {
                var repository = this.Fixture.Create(context);
                var customer = CreateValidCustomer(3);

                // Act
                await repository.AddCustomer(customer);
                var result = await repository.UniqueCustomer("Unique", "Name");

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task UniqueCustomer_CustomerExists()
        {
            using (var context = new CustomerDbContext(this.Fixture.DbContext))
            {
                var repository = this.Fixture.Create(context);
                var customer = CreateValidCustomer(4);

                // Act
                await repository.AddCustomer(customer);
                var result = await repository.UniqueCustomer(customer.FirstName, customer.LastName);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task UniqueCustomer_ThrowsException()
        {
            // Arrange
            // Force an exception by creating a DbContext without a database
            var dbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .Options;

            using (var context = new CustomerDbContext(dbContext))
            {
                var repository = this.Fixture.Create(context);

                // Act
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UniqueCustomer("Any", "Name"));

                // Assert
                Assert.NotNull(exception);
                this.Fixture.LoggerMock.VerifyWasCalledContains("Error checking customer name in Customer database context; No database provider has been configured for this DbContext.", LogLevel.Error);
            }
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
