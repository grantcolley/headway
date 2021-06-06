using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IPermissionable
    {
        bool IsPermitted(IEnumerable<string> permissions);
    }
}
