namespace Assessment.Tests.Controllers
{
    using AgData_Assessment.Controllers;
    using Assessment.Cache;
    using Assessment.Models;
    using Assessment.Services;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomersControllerTestFixture
    {
        private readonly Mock<ICustomerService> customerServiceMock;
        private readonly Mock<ICustomerCacheManager> customerCacheManagerMock;

        public Mock<ILogger<CustomersController>> LoggerMock { get; }

        public CustomersControllerTestFixture()
        {
            this.customerServiceMock = new Mock<ICustomerService>();
            this.customerCacheManagerMock = new Mock<ICustomerCacheManager>();
            this.LoggerMock = new Mock<ILogger<CustomersController>>();
        }

        public CustomersController Create()
        {
            return new CustomersController(this.customerServiceMock.Object, this.customerCacheManagerMock.Object, this.LoggerMock.Object);
        }

        public void SetupGetCustomers(List<Customer>? expectedCustomers)
        {
            this.customerCacheManagerMock.Setup(_ => _.GetCustomers()).ReturnsAsync(expectedCustomers);
        }

        public void SetupGetCustomerById(int customerId, Customer? expectedCustomer)
        {
            this.customerCacheManagerMock.Setup(_ => _.GetCustomer(customerId)).ReturnsAsync(expectedCustomer);
        }

        public void SetupCreateCustomer(Customer createdCustomer)
        {
            this.customerServiceMock.Setup(_ => _.AddCustomer(It.IsAny<CustomerInput>())).ReturnsAsync(createdCustomer);
        }

        public void SetupUpdateCustomer(Customer updatedCustomer)
        {
            this.customerServiceMock.Setup(_ => _.UpdateCustomer(It.IsAny<CustomerInput>(), It.IsAny<Customer>())).ReturnsAsync(updatedCustomer);
        }

        public void SetupDeleteCustomerById()
        {
            this.customerCacheManagerMock.Setup(_ => _.DeleteCustomer(It.IsAny<Customer>()));
        }

        public void SetupUniqueCustomer(bool isUnique)
        {
            this.customerCacheManagerMock.Setup(_ => _.UniqueCustomer(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(isUnique);
        }

        public void SetupGetCustomersThrowsException(string exceptionMessage)
        {
            this.customerCacheManagerMock.Setup(_ => _.GetCustomers()).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupGetCustomerByIdThrowsException(string exceptionMessage)
        {
            this.customerCacheManagerMock.Setup(_ => _.GetCustomer(It.IsAny<int>())).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupCreateCustomerThrowsException(string exceptionMessage)
        {
            this.customerServiceMock.Setup(_ => _.AddCustomer(It.IsAny<CustomerInput>())).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupDeleteCustomerThrowsException(string exceptionMessage)
        {
            this.customerCacheManagerMock.Setup(_ => _.DeleteCustomer(It.IsAny<Customer>())).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupUpdateCustomerThrowsException(string exceptionMessage)
        {
            this.customerServiceMock.Setup(_ => _.UpdateCustomer(It.IsAny<CustomerInput>(), It.IsAny<Customer>())).ThrowsAsync(new Exception(exceptionMessage));
        }

        public void SetupUniqueCustomerThrowsException(string exceptionMessage)
        {
            this.customerCacheManagerMock.Setup(_ => _.UniqueCustomer(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception(exceptionMessage));
        }
    }
}
