using Headway.Core.Enums;
using Headway.Core.Interface;

namespace Headway.Core.Model
{
    public class Fragment : IFragment
    {
        public int FrameId { get; set; }
        public int FragmentId { get; set; }
        public int FragmentContainerId { get; set; }
        public string FragmentName { get; set; }
        public FragmentType FragmentType { get; set; }
    }
}