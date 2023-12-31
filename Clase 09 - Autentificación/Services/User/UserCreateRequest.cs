using Entities;
using Logic;

namespace Logic.User
{
    public class UserCreateRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string? Role { get; set; } = "";

        public UserItem ToUserItem()
        {
            var userItem = new UserItem();

            userItem.Username = Username;
            userItem.PasswordHash = PasswordHash.Hash(Password);
            userItem.DisplayName = DisplayName;
            userItem.Role = Role;

            return userItem;
        }
    }
}
