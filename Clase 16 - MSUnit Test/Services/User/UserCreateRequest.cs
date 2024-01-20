using Entities.User;
using Logic;

namespace Logic.User
{
    public class UserCreateRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string? Role { get; set; } = "";

        public UserCreateDTO ToUserCreateDTO()
        {
            var userCreateDTO = new UserCreateDTO();

            userCreateDTO.Username = Username;
            userCreateDTO.PasswordHash = PasswordHash.Hash(Password);
            userCreateDTO.DisplayName = DisplayName;
            userCreateDTO.Role = Role;

            return userCreateDTO;
        }
    }
}
