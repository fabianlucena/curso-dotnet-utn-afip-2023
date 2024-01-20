namespace Entities.User
{
    public class UserCreateDTO
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string? Role { get; set; } = "";

        public UserItem ToUserItem()
        {
            var userItem = new UserItem();

            userItem.Username = Username;
            userItem.PasswordHash = PasswordHash;
            userItem.DisplayName = DisplayName;
            userItem.Role = Role;

            return userItem;
        }
    }
}