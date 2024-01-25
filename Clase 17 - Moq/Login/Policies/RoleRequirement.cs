using Microsoft.AspNetCore.Authorization;

// Clase 14: Autorización basada en políticas, definimos un requerimiento personalizado para nuestra política

namespace Login.Policies
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public RoleRequirement(string role)
        {

            Role = role;
        }
    }
}
