using Headway.Core.Model;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class AuthorisationExtensions
    {
        public static bool IsUserAuthorised(this Authorisation authorisation, string permission)
        {
            if(string.IsNullOrEmpty(permission))
            {
                return true;
            }

            return authorisation.Permissions.Any(p => p.Equals(permission));
        }
    }
}
