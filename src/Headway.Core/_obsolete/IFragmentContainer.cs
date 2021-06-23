using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFragmentContainer : IFragment
    {
        List<IFragment> Fragments { get; set; }
    }
}
