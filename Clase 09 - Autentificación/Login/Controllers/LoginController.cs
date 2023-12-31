using Logic.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ISessionLogic _sessionLogic;

        public LoginController(
            ILogger<LoginController> logger,
            ISessionLogic sessionLogic)
        {
            _logger = logger;
            _sessionLogic = sessionLogic;
        }

        // Clase 9: La decoración [AllowAnonymous] indica que para este endpoint específico no es necesario estar autentificado independientemente de la configuración del controlador
        [AllowAnonymous]
        [HttpPost]
        public LoginResponse Login(LoginRequest user)
        {
            return _sessionLogic.Login(user);
        }
    }
}
