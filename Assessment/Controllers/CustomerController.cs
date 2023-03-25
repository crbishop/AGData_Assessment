namespace AgData_Assessment.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Customer Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        public CustomerController(ILogger<CustomerController> logger)
        {
            this.logger = logger;
        }
    }
}