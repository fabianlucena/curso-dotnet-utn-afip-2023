using Data;
using Logic.User;
using Login;
using Login.Controllers;
using Login.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Login_Test
{
    public class UserControllerTest
    {
        private readonly UserController _userController;

        public UserControllerTest()
        {
            ILogger<UserController> logger = new NullLogger<UserController>();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseOracle("Data Source=localhost:1521/XEPDB1; User Id=\"curso_afip\"; Password=miedBEaUZp4vhG0S2EOH;");
            DataContext dataContext = new DataContext(optionsBuilder.Options);
            UserLogic userLogic = new UserLogic(dataContext);

            Configuration configuration = new();

            _userController = new UserController(logger, userLogic, configuration);
        }

        [Fact]
        public void Get_Success()
        {
            // Arrange - Preparar

            // Act - Ejecutar
            var result = _userController.Get();

            // Assert - Verificar
            Assert.IsAssignableFrom<IEnumerable<UserResponse>>(result);
        }

        [Fact]
        public void Get_ManyUsers()
        {
            // Arrange - Preparar
            int minExpectedCount = 2,
                maxExpectedCount = 20;

            // Act - Ejecutar
            var result = _userController.Get();

            // Assert - Verificar
            var users = Assert.IsAssignableFrom<IEnumerable<UserResponse>>(result);
            Assert.InRange(users.ToList().Count, minExpectedCount, maxExpectedCount);
        }

        [Fact]
        public void GetForId_Success()
        {
            // Arrange - Preparar
            int id = 41;

            // Act - Ejecutar
            var result = _userController.Get(id);

            // Assert - Verificar
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        /*[Fact]
        public void GetForId_Null()
        {
            // Arrange - Preparar
            int id = -1;

            // Act - Ejecutar
            UserResponse? result;
            try
            {
                result = _userController.Get(id);
            } catch(Exception)
            {
                result = null;
            }

            // Assert - Verificar
            Assert.Null(result);
        }*/

        [Fact]
        public void GetForId_NotFound()
        {
            // Arrange - Preparar
            int id = -1;

            // Act - Ejecutar
            var result = _userController.Get(id);

            // Assert - Verificar
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetForId_CheckIdValue()
        {
            // Arrange - Preparar
            int id = 451;

            // Act - Ejecutar
            var result = _userController.Get(id);

            // Assert - Verificar
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserResponse>(okObjectResult.Value);
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }
    }
}