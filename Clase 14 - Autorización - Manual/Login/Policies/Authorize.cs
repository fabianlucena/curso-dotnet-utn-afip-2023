using Entities;
using Logic.User;
using Login.Exceptions;

// Clase Autorizador personalizado.

namespace Login.Policies
{
    public class Authorize : IAuthorize
    {
        private readonly IUserLogic _userLogic;
        private readonly IHttpContextAccessor _contextAccessor;

        public Authorize(IUserLogic userLogic, IHttpContextAccessor contextAccessor)
        {
            _userLogic = userLogic;
            _contextAccessor = contextAccessor;
        }

        public void CheckPermission(string permission)
        {
            var context = _contextAccessor.HttpContext;
            string? userId = context?.User.Claims.First(c => c.Type == "userId")?.Value;
            if (userId == null || string.IsNullOrWhiteSpace(userId))
            {
                throw new HttpException(401, "Forbiden 01");
            }

            UserItem? user = _userLogic.GetForId(long.Parse(userId ?? "0"));
            List<string> roles = user?.Role?.Split(",")
                .ToList()
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList() ?? new List<string>();
            if (roles.Any(r => r == permission))
            {
                return;
            }

            throw new HttpException(401, "Forbiden 02");
        }
    }
}
