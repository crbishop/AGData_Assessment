namespace Assessment.Tests.Services
{
    using Assessment.Cache;
    using Assessment.Mappers;
    using Assessment.Models;
    using Assessment.Services;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomerServiceTestFixture
    {
        public Mock<ICustomerCacheManager> CustomerCacheManagerMock { get; }
        public IMapper Mapper { get; set; }
        public Mock<ILogger<ICustomerService>> LoggerMock { get; }

        public CustomerServiceTestFixture()
        {
            this.CustomerCacheManagerMock = new Mock<ICustomerCacheManager>();
            this.LoggerMock = new Mock<ILogger<ICustomerService>>();

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
            this.Mapper = mapperConfig.CreateMapper();
        }
     
        public ICustomerService Create()
        {
            return new CustomerService(this.CustomerCacheManagerMock.Object, this.Mapper, this.LoggerMock.Object);
        }

        public void SetupAddCustomer()
        {
            this.CustomerCacheManagerMock.Setup(_ => _.AddCustomer(It.IsAny<Customer>()));
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
            this.CustomerCacheManagerMock.Reset();
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
