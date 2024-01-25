using Entities.User;

namespace Logic.User
{
    public interface IUserLogic
    {
        UserItem Create(UserCreateRequest user);
        UserItem? GetForId(long id);
        UserItem? GetForUsername(string username);
        List<UserItem> GetList();
        void Delete(long id);
    }
}
