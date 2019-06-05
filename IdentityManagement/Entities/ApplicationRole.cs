using Microsoft.AspNet.Identity;

namespace IdentityManagement.Entities
{
    public class ApplicationRole : IRole<string>
    {
        public string Id { get => RoleId; set => RoleId = value; }

        public string Name { get => RoleName; set => RoleName = value; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RowStatus { get; set; }
    }
}
