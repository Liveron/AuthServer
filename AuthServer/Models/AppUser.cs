using Microsoft.AspNetCore.Identity;

namespace AuthServer.Models
{
    public class AppUser : IdentityUser<Guid>
    {

        public string Name { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
    }
}
