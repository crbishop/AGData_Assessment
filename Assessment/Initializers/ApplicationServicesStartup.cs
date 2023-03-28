namespace Assessment.Initializers
{
    using Assessment.Services;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Container for application services DI Container logic.
    /// </summary>
    public static class ApplicationServicesStartup
    {
        /// <summary>
        /// Registers all the components required for the application services.
        /// </summary>
        /// <param name="serviceCollection">Service Collection.</param>
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<ICustomerService, CustomerService>();
        }
    }
}
