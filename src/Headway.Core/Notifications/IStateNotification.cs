using System;

namespace Headway.Core.Notifications
{
    public interface IStateNotification
    {
        void Register(string target, Action action);
        void Deregister(string target);
        void NotifyStateHasChanged(string target);
    }
}
