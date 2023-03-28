namespace Assessment.Tests.Mappers
{
    using Assessment.Mappers;
    using Assessment.Models;
    using AutoMapper;
    using Xunit;

    public class CustomerProfileTest
    {
        public IMapper Mapper { get; }

        public CustomerProfileTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
            this.Mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void CustomerInputToCustomerIsMapped()
        {
            var customerInput = new CustomerInput
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Test Ave",
            };

            var result = this.Mapper.Map<Customer>(customerInput);

            Assert.Equal(customerInput.FirstName, result.FirstName);
            Assert.Equal(customerInput.LastName, result.LastName);
            Assert.Equal(customerInput.Address, result.Address);
            
            // Verify that the Created datetime is recent
            Assert.True(DateTime.Now.AddSeconds(-5).Ticks < result.Created.Ticks);
        }
    }
}
