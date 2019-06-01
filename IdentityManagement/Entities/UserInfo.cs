using IdentityManagement.Utilities;
using Microsoft.AspNet.Identity;

namespace IdentityManagement.Entities
{
    public class UserInfo : IUser<string>
    {
        public string Id { get => UserId; set => UserId = value; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EnumUserStatus UserStatus { get; set; }
        public string RowStatus { get; set; }
    }
}
