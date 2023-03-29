namespace Assessment.Tests.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Assessment.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class CustomersControllerTest : IClassFixture<CustomersControllerTestFixture>
    {
        private readonly CustomersControllerTestFixture fixture;

        public CustomersControllerTest(CustomersControllerTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetCustomers_Success()
        {
            // Arrange
            var customerList = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "123 Test Ave",
                }
            };

            this.fixture.SetupGetCustomers(customerList);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.GetCustomers();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.Same(customerList, okResult.Value);
        }

        [Fact]
        public async Task GetCustomers_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomersThrowsException(exceptionMessage);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.GetCustomers();

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

            this.fixture.LoggerMock.VerifyWasCalledEquals($"Error retrieving list of all customers; {exceptionMessage}", LogLevel.Error);
        }

        [Fact]
        public async Task GetCustomerById_Success()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Test Ave",
            };

            this.fixture.SetupGetCustomerById(customerId, customer);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.Same(customer, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerById_NullResponse()
        {
            // Arrange
            var customerId = 1;

            this.fixture.SetupGetCustomerById(customerId, null);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCustomerById_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Exception message";
            this.fixture.SetupGetCustomerByIdThrowsException(exceptionMessage);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.GetCustomer(1);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

            this.fixture.LoggerMock.VerifyWasCalledEquals($"Error retrieving customer data; {exceptionMessage}", LogLevel.Error);
        }

        [Fact]
        public async Task CreateCustomer_Success()
        {
            // Arrange
            var customerInput = new CustomerInput
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Test Ave",
            };

            this.fixture.SetupUniqueCustomer(true);
            this.fixture.SetupCreateCustomer(customer);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(customerInput);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);

            var createdResult = (CreatedAtActionResult)result;
            Assert.Same(customer, createdResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_NullInput()
        {
            // Arrange
            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(null);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal("Customer input cannot be null.", objectResult.Value);
        }

        [Theory]
        [InlineData("John", "")]
        [InlineData("", "Doe")]
        [InlineData("", "")]
        public async Task CreateCustomer_EmptyName(string firstname, string lastname)
        {
            // Arrange
            var customerInput = new CustomerInput
            {
                FirstName = firstname,
                LastName = lastname,
            };

            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(customerInput);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal("Customer name cannot be null or empty.", objectResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_NameExists()
        {
            // Arrange
            var customerInput = new CustomerInput
            {
                FirstName = "John",
                LastName = "Doe",
            };

            this.fixture.SetupUniqueCustomer(false);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(customerInput);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var customerName = customerInput.FirstName + " " + customerInput.LastName;
            Assert.Equal($"Customer first and last name ({customerName}) already exists.", objectResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_ThrowsException()
        {
            // Arrange
            var customerInput = new CustomerInput
            {
                FirstName = "John",
                LastName = "Doe",
            };

            this.fixture.SetupUniqueCustomer(true);

            var exceptionMessage = "Exception message";
            this.fixture.SetupCreateCustomerThrowsException(exceptionMessage);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(customerInput);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

            this.fixture.LoggerMock.VerifyWasCalledEquals($"Error creating a new customer; {exceptionMessage}", LogLevel.Error);
        }

        [Fact]
        public async Task ValidateUniqueCustomer_ThrowsException()
        {
            // Arrange
            var customerInput = new CustomerInput
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var exceptionMessage = "Exception message";
            this.fixture.SetupUniqueCustomerThrowsException(exceptionMessage);

            var controller = this.fixture.Create();

            // Act
            var result = await controller.CreateCustomer(customerInput);

            // Assert
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

            this.fixture.LoggerMock.VerifyWasCalledEquals($"Error creating a new customer; {exceptionMessage}", LogLevel.Error);
        }
    }
}
