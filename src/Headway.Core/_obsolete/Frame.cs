using Headway.Core.Interface;
using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Frame : IFrame
    {
        public int FrameId { get; set; }
        public string FrameName { get; set; }
        public IFragmentContainer RootFragmentContainer { get; set; }
        public List<IFragment> Fragments { get; set; }
    }
}
