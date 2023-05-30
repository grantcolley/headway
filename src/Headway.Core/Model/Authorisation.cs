using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Authorisation
    {
        public Authorisation() 
        {
            Permissions = new List<string>();
        }

        public string User { get; set; }
        public List<string> Permissions { get; set; }
    }
}
