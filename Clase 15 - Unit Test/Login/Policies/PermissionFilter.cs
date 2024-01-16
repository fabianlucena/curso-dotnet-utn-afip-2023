using Entities;
using Logic.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// Clase 14: Filtro de permisos, no es necesario agregarlo en el Program.cs, porque se agrega automáticametne al implemementar la interface IAuthorizationFilter

namespace Login.Policies
{
    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IUserLogic _userLogic;

        public PermissionFilter(string permission, IUserLogic userLogic)
        {
            _permission = permission;
            _userLogic = userLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? userId = context.HttpContext.User.Claims.First(c => c.Type == "userId")?.Value;
            if (userId == null || string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }

            UserItem? user = _userLogic.GetForId(long.Parse(userId ?? "0"));
            List<string> roles = user?.Role?.Split(",")
                .ToList()
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList() ?? new List<string>();
            if (roles.Any(r => r == _permission))
            {
                return;
            }

            context.Result = new UnauthorizedObjectResult(string.Empty);
            return;
        }
    }
}
