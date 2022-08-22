using Microsoft.AspNetCore.Identity;

namespace ShopApp.WebUI.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string  FullName { get; set; }
    }
}
