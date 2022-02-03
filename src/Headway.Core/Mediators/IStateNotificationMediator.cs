using System;

namespace Headway.Core.Mediators
{
    public interface IStateNotificationMediator
    {
        void Register(string target, Action action);
        void Deregister(string target);
        void NotifyStateHasChanged(string target);
    }
}
