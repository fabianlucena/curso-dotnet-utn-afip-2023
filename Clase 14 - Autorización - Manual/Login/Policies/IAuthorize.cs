// Clase 14: Interfaz del autorizador personalizado.

namespace Login.Policies
{
    public interface IAuthorize
    {
        public void CheckPermission(string permission);
    }
}
