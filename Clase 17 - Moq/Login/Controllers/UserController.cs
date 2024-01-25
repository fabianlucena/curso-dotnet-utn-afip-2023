using Microsoft.AspNetCore.Mvc;
using Logic.User;
using Login.DTO;
using Microsoft.AspNetCore.Authorization;
using Login.Exceptions;
using Entities.User;

namespace Login.Controllers
{
    // Clase 9: La decoración [Authorize] indica que para los endpoints definidos en este controlador hay que estar autentificado
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserLogic _userLogic;
        private readonly Configuration _configuration;

        public UserController(
            ILogger<UserController> logger,
            IUserLogic userLogic,
            Configuration configuration)
        {
            _logger = logger;
            _userLogic = userLogic;global 
            _configuration = configuration;
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
        //[Authorize(Policy = "AdminRole")] // Clase 14: Autorización basada en políticas, acá se coloca el nombre de la política, como limitación no pueden pasarse parámetros.
        public IEnumerable<UserResponse> Get()
        {
            return _userLogic.GetList()
                .Select(u => new UserResponse(u))
                .ToArray();
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            UserItem? user = _userLogic.GetForId(id);
            if (user == null)
            {
                //throw new HttpException(404, "El ususario no existe");
                return NotFound(new { message = "El usuario no se encuentra." });
            }

            return Ok(new UserResponse(user));
        }

        [HttpDelete]
        public void Delete(long id)
        {
            _userLogic.Delete(id);
        }
    }
}
