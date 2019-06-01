using Microsoft.AspNet.Identity;

namespace IdentityManagement.Entities
{
    public class RoleInfo : IRole<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
