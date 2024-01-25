using Entities.User;

namespace MemBD
{
    public class UserMDB : IUserEntity
    {
        static List<UserItem> _users = new List<UserItem>();

        public UserItem Create(UserCreateDTO user)
        {
            UserItem userItem = user.ToUserItem();
            if (_users.Count > 0)
            {
                userItem.Id = _users.Max(x => x.Id) + 1;
            }
            else
            {
                userItem.Id = 1;
            }
            _users.Add(userItem);
            return userItem;
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public UserItem? GetForId(long id)
        {
            return _users.Where(u =>  u.Id == id).FirstOrDefault();
        }

        public UserItem? GetForUsername(string username)
        {
            return _users.Where(u => u.Username == username).FirstOrDefault();
        }

        public List<UserItem> GetList()
        {
            return _users.ToList();
        }
    }
}
