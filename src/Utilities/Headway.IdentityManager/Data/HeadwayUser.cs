using Microsoft.AspNetCore.Identity;

namespace Headway.IdentityManager.Data
{
    public class HeadwayUser
    {
        public HeadwayUser(IdentityUser identityUser)
        {
            IdentityUser = identityUser;
        }

        public IdentityUser IdentityUser { get; }
        public string Roles { get; set; }

        public string Id
        {
            get { return IdentityUser?.Id; }
            set { IdentityUser.Id = value; }
        }

        public string Email
        {
            get { return IdentityUser?.Email; }
            set { IdentityUser.Email = value; }
        }

        public string UserName 
        {
            get { return IdentityUser?.UserName; }
            set { IdentityUser.UserName = value; }
        }
    }
}
