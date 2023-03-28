namespace Assessment
{
    /// <summary>
    /// The start file of the project.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        protected Program()
        {
        }

        /// <summary>
        /// The Main entry point.
        /// </summary>
        /// <param name="args">args.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///  Part of startup and used by Main.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>none.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}