namespace Logic.Session
{
    // Clase 12: Se ha segregado la interface ILogin de ISession
    public interface ILoginLogic
    {
        LoginResponse Login(LoginRequest login);
    }
}
