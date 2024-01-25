using Entities.User;

namespace Data
{
    public class UserDB : IUserEntity
    {
        private readonly DataContext _dataContext;
        
        public UserDB(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public UserItem Create(UserCreateDTO user)
        {
            UserItem userItem = user.ToUserItem();
            _dataContext.Users.Add(userItem);
            _dataContext.SaveChanges();
            return userItem;
        }

        public void Delete(long id)
        {
            UserItem? user = GetForId(id);
            if (user == null)
            {
                throw new ArgumentException("The user with that ID does not exist.");
            }

            user.ExpiredAt = DateTime.Now;

            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }

        public UserItem? GetForId(long id)
        {
            return (from u in _dataContext.Users
                    where (u.ExpiredAt == null || u.ExpiredAt > DateTime.Now)
                        && u.Id == id
                    select u).FirstOrDefault();
        }

        public UserItem? GetForUsername(string username)
        {
            return (from u in _dataContext.Users
                    where (u.ExpiredAt == null || u.ExpiredAt > DateTime.Now)
                        && u.Username == username
                    select u).FirstOrDefault();
        }

        public List<UserItem> GetList()
        {
            return (from u in _dataContext.Users
                    where u.ExpiredAt == null || u.ExpiredAt > DateTime.Now
                    select u).ToList();
        }
    }
}
