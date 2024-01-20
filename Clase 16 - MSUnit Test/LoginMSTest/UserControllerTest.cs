using Entities.User;
using Logic.User;
using Login;
using Login.Controllers;
using Login.DTO;
using MemBD;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoginMSTest
{
    [TestClass]
    public class UserControllerTest
    {
        public readonly UserController _userController;

        public UserControllerTest()
        {
            ILogger<UserController> logger = new NullLogger<UserController>();
            Configuration configuration = new Configuration();
            IUserEntity userEntity = new UserMDB();
            IUserLogic userLogic = new UserLogic(userEntity);
            _userController = new UserController(logger, userLogic, configuration);
        }

        [TestMethod]
        public void Create_Ok()
        {
            // Arrange - Preparar
            UserCreateRequest request = new UserCreateRequest()
            {
                Username = "prueba",
                Password = "1234",
                DisplayName = "Prueba",
                Role = "operator"
            };

            // Act - Ejecutar
            var result = _userController.Create(request);

            // Assert - Verificar
            Assert.IsInstanceOfType(result, typeof(UserResponse));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Duplicate()
        {
            // Arrange - Preparar
            UserCreateRequest request = new UserCreateRequest()
            {
                Username = "prueba01",
                Password = "1234",
                DisplayName = "Prueba 01",
                Role = "operator"
            };

            // Act - Ejecutar
            var result = _userController.Create(request);
            result = _userController.Create(request);

            // Assert - Verificar
        }

        [TestMethod]
        [DataRow("prueba02", "1234", "Prueba 02", "operator")]
        [DataRow("prueba03", "1234", "Prueba 02", "operator")]
        [DataRow("prueba04", "1234", "Prueba 03", "operator")]
        [DataRow("prueba05", "1234", "Prueba 04", "operator")]
        public void Create_Success(string username, string password, string displayName, string role)
        {
            // Arrange - Preparar
            UserCreateRequest request = new UserCreateRequest()
            {
                Username = username,
                Password = password,
                DisplayName = displayName,
                Role = role
            };

            // Act - Ejecutar
            var result = _userController.Create(request);

            // Assert - Verificar
        }
    }
}