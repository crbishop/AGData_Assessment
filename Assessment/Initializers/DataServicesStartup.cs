namespace Assessment.Initializers
{
    using Assessment.Repositories;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Container for data services DI Container logic.
    /// </summary>
    public static class DataServicesStartup
    {
        /// <summary>
        /// Registers all the components required for the data services.
        /// </summary>
        /// <param name="serviceCollection">Service Collection.</param>
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
