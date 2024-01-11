using Microsoft.AspNetCore.Mvc;
using Logic.User;
using Login.DTO;
using Microsoft.AspNetCore.Authorization;
using Login.Exceptions;
using System.Runtime.CompilerServices;
using Login.Policies;

namespace Login.Controllers
{
    // Clase 9: La decoración [Authorize] indica que para los endpoints definidos en este controlador hay que estar autentificado
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserLogic _userLogic;
        private readonly Configuration _configuration;
        private readonly IAuthorize _authorize; // Clase 14: la autorización se hace a traves de la calse personlizada.

        public UserController(
            ILogger<UserController> logger,
            IUserLogic userLogic,
            Configuration configuration,
            IAuthorize authorize)
        {
            _logger = logger;
            _userLogic = userLogic;
            _configuration = configuration;
            _authorize = authorize;
        }

        [HttpPost]
        public UserResponse Create(UserCreateRequest user)
        {
            if (_configuration.AddUserDisabled)
            {
                throw new HttpException(403, "Addign user is disabled");
            }

            return new UserResponse(_userLogic.Create(user));
        }

        [HttpGet]
        public IEnumerable<UserResponse> Get()
        {
            _authorize.CheckPermission("admin");

            return _userLogic.GetList()
                .Select(u => new UserResponse(u))
                .ToArray();
        }

        [HttpDelete]
        public void Delete(long id)
        {
            _userLogic.Delete(id);
        }
    }
}
