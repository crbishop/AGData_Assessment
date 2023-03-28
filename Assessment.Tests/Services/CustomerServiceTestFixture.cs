namespace Assessment.Tests.Services
{
    using Assessment.Mappers;
    using Assessment.Models;
    using Assessment.Repositories;
    using Assessment.Services;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomerServiceTestFixture
    {
        public Mock<ICustomerRepository> CustomerRepositoryMock { get; }
        public IMapper Mapper { get; set; }
        public Mock<ILogger<ICustomerService>> LoggerMock { get; }

        public CustomerServiceTestFixture()
        {
            this.CustomerRepositoryMock = new Mock<ICustomerRepository>();
            this.LoggerMock = new Mock<ILogger<ICustomerService>>();

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
            this.Mapper = mapperConfig.CreateMapper();
        }
     
        public ICustomerService Create()
        {
            return new CustomerService(this.CustomerRepositoryMock.Object, this.Mapper, this.LoggerMock.Object);
        }

        public void SetupAddCustomer()
        {
            this.CustomerRepositoryMock.Setup(_ => _.AddCustomer(It.IsAny<Customer>()));
        }

        public void ResetAutoMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
            this.Mapper = mapperConfig.CreateMapper();
        }

        public void SetupBadAutoMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<BadCustomerProfile>());
            this.Mapper = mapperConfig.CreateMapper();
        }

        public void ResetRepositoryMocks()
        {
            this.CustomerRepositoryMock.Reset();
            this.SetupAddCustomer();
        }
    }

    internal class BadCustomerProfile : Profile
    {
        public BadCustomerProfile()
        {
            // This mapping is missing the Customer object, so mapping will fail for testing purposes
            this.CreateMap<CustomerInput, CustomerInput>();
        }
    }
}
