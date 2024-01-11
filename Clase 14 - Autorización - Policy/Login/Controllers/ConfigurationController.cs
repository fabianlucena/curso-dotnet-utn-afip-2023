using Logic.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly Configuration _configuration;

        public ConfigurationController(
            ILogger<ConfigurationController> logger,
            Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public void EnableAddingUser()
        {
            _configuration.AddUserDisabled = false;
        }
    }
}
