using Microsoft.AspNetCore.Identity;

namespace Library.Management.Demo.Models
{
    public class User : IdentityUser
    {
        public string Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
