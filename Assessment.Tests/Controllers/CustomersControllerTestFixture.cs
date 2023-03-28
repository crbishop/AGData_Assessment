namespace Assessment.Tests.Controllers
{
    using AgData_Assessment.Controllers;
    using Assessment.Models;
    using Assessment.Repositories;
    using Assessment.Services;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomersControllerTestFixture
    {
        private readonly Mock<ICustomerService> customerServiceMock;
        private readonly Mock<ICustomerRepository> customerRepositoryMock;

        public Mock<ILogger<CustomersController>> LoggerMock { get; }

        public CustomersControllerTestFixture()
        {
            this.customerServiceMock = new Mock<ICustomerService>();
            this.customerRepositoryMock = new Mock<ICustomerRepository>();
            this.LoggerMock = new Mock<ILogger<CustomersController>>();
        }

        public CustomersController Create()
        {
            return new CustomersController(this.customerServiceMock.Object, this.customerRepositoryMock.Object, this.LoggerMock.Object);
        }

        public void SetupGetCustomerById(int customerId, Customer? expectedCustomer)
        {
            this.customerRepositoryMock.Setup(_ => _.GetCustomer(customerId)).ReturnsAsync(expectedCustomer);
        }

        public void SetupCreateCustomer(Customer expectedCustomer)
        {
            this.customerServiceMock.Setup(_ => _.AddCustomer(It.IsAny<CustomerInput>())).ReturnsAsync(expectedCustomer);
        }

        public void SetupGetCustomerByIdThrowsException(string exceptionMessage)
        {
            this.customerRepositoryMock.Setup(_ => _.GetCustomer(It.IsAny<int>())).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupCreateCustomerThrowsException(string exceptionMessage)
        {
            this.customerServiceMock.Setup(_ => _.AddCustomer(It.IsAny<CustomerInput>())).ThrowsAsync(new Exception(exceptionMessage));
        }
    }
}
