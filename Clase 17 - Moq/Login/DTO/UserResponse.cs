using Entities.User;

namespace Login.DTO
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Role { get; set; }

        public UserResponse(UserItem userItem)
        {
            Id = userItem.Id;
            Username = userItem.Username;
            DisplayName = userItem.DisplayName;
            Role = userItem.Role;
        }
    }
}
