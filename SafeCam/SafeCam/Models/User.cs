using Microsoft.AspNetCore.Identity;

namespace SafeCam.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }

    }
}
