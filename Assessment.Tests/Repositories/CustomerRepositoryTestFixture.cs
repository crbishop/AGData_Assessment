namespace Assessment.Tests.Repositories
{
    using Assessment.Data;
    using Assessment.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CustomerRepositoryTestFixture
    {
        public Mock<ILogger<ICustomerRepository>> LoggerMock { get; }
        public DbContextOptions<CustomerDbContext> DbContext { get; }

        public CustomerRepositoryTestFixture()
        {
            this.LoggerMock = new Mock<ILogger<ICustomerRepository>>();

            this.DbContext = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: "CustomerTestSample")
                .Options;
        }

        public CustomerRepository Create(CustomerDbContext dbContext)
        {
            return new CustomerRepository(dbContext, this.LoggerMock.Object);
        }
    }
}
