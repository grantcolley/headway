using Headway.Core.Enums;

namespace Headway.Core.Interface
{
    public interface IFragment
    {
        int FrameId { get; set; }
        int FragmentId { get; set; }
        int FragmentContainerId { get; set; }
        string FragmentName { get; set; }
        FragmentType FragnmentType { get; set; }
    }
}
