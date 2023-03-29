namespace Assessment.Tests.Services
{
    using Assessment.Models;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class CustomerServiceTest : IClassFixture<CustomerServiceTestFixture>
    {
        private readonly CustomerServiceTestFixture fixture;

        public CustomerServiceTest(CustomerServiceTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task AddCustomer_Success()
        {
            // Arrange
            this.fixture.ResetRepositoryMocks();
            var service = this.fixture.Create();

            // Act
            await service.AddCustomer(new CustomerInput());

            // Assert
            this.fixture.CustomerCacheManagerMock.Verify(_ => _.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task AddCustomer_AutoMapperThrowsException()
        {
            // Arrange
            this.fixture.SetupBadAutoMapper();
            var service = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<AutoMapper.AutoMapperMappingException>(() => service.AddCustomer(new CustomerInput()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains("Error mapping customer add input; Missing type map configuration or unsupported mapping", LogLevel.Error);

            // Cleanup
            this.fixture.ResetAutoMapper();
        }

        [Fact]
        public async Task UpdateCustomer_Success()
        {
            // Arrange
            this.fixture.ResetRepositoryMocks();
            var service = this.fixture.Create();

            // Act
            await service.UpdateCustomer(new CustomerInput(), new Customer());

            // Assert
            this.fixture.CustomerCacheManagerMock.Verify(_ => _.UpdateCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomer_AutoMapperThrowsException()
        {
            // Arrange
            this.fixture.SetupBadAutoMapper();
            var service = this.fixture.Create();

            // Act
            var exception = await Assert.ThrowsAsync<AutoMapper.AutoMapperMappingException>(() => service.UpdateCustomer(new CustomerInput(), new Customer()));

            // Assert
            Assert.NotNull(exception);
            this.fixture.LoggerMock.VerifyWasCalledContains("Error mapping customer update input; Missing type map configuration or unsupported mapping", LogLevel.Error);

            // Cleanup
            this.fixture.ResetAutoMapper();
        }
    }
}
