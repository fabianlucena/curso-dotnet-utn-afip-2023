using Entities.User;
using Logic;
using Logic.User;
using Login;
using Login.Controllers;
using Login.DTO;
using MemBD;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Data;

namespace LoginMSTest
{
    [TestClass]
    public class PasswordHasshTest
    {
        public PasswordHasshTest()
        {
        }

        [TestMethod]
        [DataRow("1234")]
        [DataRow("Dios")]
        [DataRow("Amor")]
        [DataRow("Sexo")]
        [DataRow("Secreto")]
        public void HashAndVerify_Ok(string password)
        {
            // Arrange - Preparar
            string hash = PasswordHash.Hash(password);

            // Act - Ejecutar
            var result = PasswordHash.Verify(password, hash);

            // Assert - Verificar
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("1234")]
        [DataRow("Dios")]
        [DataRow("Amor")]
        [DataRow("Sexo")]
        [DataRow("Secreto")]
        public void HashAndVerify_Failure(string password)
        {
            // Arrange - Preparar
            string hash = PasswordHash.Hash(password);

            // Act - Ejecutar
            var result = PasswordHash.Verify(password + "3", hash);

            // Assert - Verificar
            Assert.IsFalse(result);
        }
    }
}