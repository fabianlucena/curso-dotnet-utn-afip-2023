using Entities;
using Logic.User;
using Microsoft.AspNetCore.Authorization;

/* Clase 14: Autorización basada en políticas, manipulador de la política
 * Relacionado al requerimiento personalizado RoleRequirement, 
 * ejecuta las tareas de verificación de permisos
 */

namespace Login.Policies
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IUserLogic _userLogic;

        public RoleHandler(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            string? userId = context.User.Claims.First(c => c.Type == "userId")?.Value;
            if (userId == null || string.IsNullOrWhiteSpace(userId))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            UserItem? user = _userLogic.GetForId(long.Parse(userId));
            List<string> roles = user?.Role?.Split(",")
                .ToList()
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList() ?? new List<string>();

            if (roles.Any(r => r == requirement.Role))
            {
                context.Succeed(requirement);
                return Task.FromResult(0);
            }

            context.Fail();
            return Task.FromResult(0);
        }
    }
}
