using Headway.Core.Enums;
using Headway.Core.Interface;
using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class FragmentContainer : IFragmentContainer
    {
        public int FrameId { get; set; }
        public int FragmentId { get; set; }
        public int FragmentContainerId { get; set; }
        public string FragmentName { get; set; }
        public FragmentType FragmentType { get; set; }
        public List<IFragment> Fragments { get; set; }
    }
}
