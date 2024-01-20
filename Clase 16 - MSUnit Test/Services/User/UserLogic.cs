using Data;
using Entities.User;

namespace Logic.User
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserEntity _userEntity;

        public UserLogic(IUserEntity userEntity)
        {
            _userEntity = userEntity;
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

            UserItem? userItem = _userEntity.GetForUsername(user.Username);
            if (userItem != null)
            {
                throw new ArgumentException("A user with that username already exists.");
            }

            userItem = _userEntity.Create(user.ToUserCreateDTO());

            return userItem;
        }

        List<UserItem> IUserLogic.GetList()
        {
            return _userEntity.GetList();
        }

        UserItem? IUserLogic.GetForId(long id)
        {
            return _userEntity.GetForId(id);
        }

        UserItem? IUserLogic.GetForUsername(string username)
        {
            return _userEntity.GetForUsername(username);
        }

        void IUserLogic.Delete(long id)
        {
            _userEntity.Delete(id);
        }
    }
}
