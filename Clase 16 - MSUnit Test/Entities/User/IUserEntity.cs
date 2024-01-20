namespace Entities.User
{
    public interface IUserEntity
    {
        UserItem Create(UserCreateDTO user);
        UserItem? GetForUsername(string username);
        UserItem? GetForId(long id);
        List<UserItem> GetList();
        void Delete(long id);
    }
}
