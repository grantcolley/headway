using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class AuthorisationHelper
    {
        public static Authorisation CreateAuthorisation(List<string> permissions)
        {
            return new Authorisation
            {
                User = Environment.UserName,
                Permissions = permissions
            };
        }
    }
}
