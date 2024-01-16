using Entities;

namespace Logic.Session
{
    public interface ISessionLogic
    {
        SessionItem GetById(long id);
    }
}
