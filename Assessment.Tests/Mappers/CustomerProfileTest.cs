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
        public void NewCustomer_IsMapped()
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
            
            // Verify that the Created and Updated datetime is recent
            Assert.True(DateTime.Now.AddSeconds(-5).Ticks < result.Created?.Ticks);
            Assert.True(DateTime.Now.AddSeconds(-5).Ticks < result.Updated.Ticks);
        }

        [Fact]
        public void UpdatedCustomer_DoesNotChangeCreated()
        {
            // The Created date is only set initially and never updated
            var customerInput = new CustomerInput
            {
                FirstName = "Jane",
                LastName = "Doe",
                Address = "555 Test Ave",
            };

            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Created = DateTime.Parse("03/29/2023 01:02:03"),
            };

            var result = this.Mapper.Map(customerInput, customer);

            Assert.Equal(customerInput.FirstName, result.FirstName);
            Assert.Equal(customerInput.LastName, result.LastName);
            Assert.Equal(customerInput.Address, result.Address);

            // Verify that the Created datetime is not changed and Updated datetime is recent
            Assert.Equal(customer.Created, result.Created);
            Assert.True(DateTime.Now.AddSeconds(-5).Ticks < result.Updated.Ticks);
        }
    }
}
