using Microsoft.AspNetCore.Mvc;

// Clase 14: Attributo de permisos para usar Permission como decoración en el controlador

namespace Login.Policies
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
        {
            // Clase 14: Dado que el attributo de permisos usa argumento, hay que pasárselo al PermissonFilter
            Arguments = new object[] { permission };
        }
    }
}
