namespace Assessment
{
    using Assessment.Data;
    using Assessment.Initializers;
    using Assessment.Mappers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Includes methods to configure services and to create the application's request processing pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets Configuration property.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerDbContext>(options => options.UseInMemoryDatabase(databaseName: "Assessment"));

            ApplicationServicesStartup.Configure(services);
            DataServicesStartup.Configure(services);

            services.AddAutoMapper(typeof(CustomerProfile));

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AGData Assessment API",
                    Version = "v1.0",
                    Description = "AGData Assessment API",
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder">Application Builder.</param>
        /// <param name="environment">Environment.</param>
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseRouting();
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AGData Assessment API");
                c.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
