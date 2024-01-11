using Data;
using Entities;

namespace Logic.User
{
    public class UserLogic : IUserLogic
    {
        private readonly DataContext _dataContext;
        
        public UserLogic(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        UserItem IUserLogic.Create(UserCreateRequest user)
        {
            if (String.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentNullException(nameof(user.Username));
            }

            if (String.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentNullException(nameof(user.Password));
            }

            if (String.IsNullOrWhiteSpace(user.DisplayName))
            {
                throw new ArgumentNullException(nameof(user.DisplayName));
            }

            int count = _dataContext.Users.Where(u => u.Username == user.Username).Count();
            if (count > 0) {
                throw new ArgumentException("A user already with that username already exists.");
            }

            UserItem userItem = user.ToUserItem();
            _dataContext.Users.Add(userItem);
            _dataContext.SaveChanges();

            return userItem;
        }

        List<UserItem> IUserLogic.GetList()
        {
            return (from u in _dataContext.Users
                   where u.ExpiredAt == null || u.ExpiredAt > DateTime.Now
                   select u).ToList();
        }

        UserItem? IUserLogic.GetForId(long id)
        {
            return (from u in _dataContext.Users
                    where (u.ExpiredAt == null || u.ExpiredAt > DateTime.Now)
                        && u.Id == id
                    select u).FirstOrDefault();
        }

        UserItem? IUserLogic.GetForUsername(string username)
        {
            return (from u in _dataContext.Users
                    where (u.ExpiredAt == null || u.ExpiredAt > DateTime.Now)
                        && u.Username == username
                    select u).FirstOrDefault();
        }

        void IUserLogic.Delete(long id)
        {
            UserItem? user = ((IUserLogic)this).GetForId(id);
            if (user == null)
            {
                throw new ArgumentException("The user with that ID does not exist.");
            }

            user.ExpiredAt = DateTime.Now;

            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }
    }
}
