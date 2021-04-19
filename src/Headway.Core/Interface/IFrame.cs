using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFrame
    {
        int FrameId { get; set; }
        string FrameName { get; set; }
        IFragmentContainer RootFragmentContainer { get; set; }
        List<IFragment> Fragments { get; set; }
    }
}
