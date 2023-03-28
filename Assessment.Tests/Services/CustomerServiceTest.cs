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
            this.fixture.CustomerRepositoryMock.Verify(_ => _.AddCustomer(It.IsAny<Customer>()), Times.Once);
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
            this.fixture.LoggerMock.VerifyWasCalledContains("Error mapping customer input; Missing type map configuration or unsupported mapping", LogLevel.Error);

            // Cleanup
            this.fixture.ResetAutoMapper();
        }
    }
}
