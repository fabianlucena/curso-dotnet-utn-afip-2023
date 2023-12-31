using Microsoft.AspNetCore.Mvc;
using Logic.User;
using Login.DTO;
using Microsoft.AspNetCore.Authorization;

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

        public UserController(
            ILogger<UserController> logger,
            IUserLogic userLogic)
        {
            _logger = logger;
            _userLogic = userLogic;
        }

        [HttpPost]
        public UserResponse Create(UserCreateRequest user)
        {
            return new UserResponse(_userLogic.Create(user));
        }

        [HttpGet]
        public IEnumerable<UserResponse> Get()
        {
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
