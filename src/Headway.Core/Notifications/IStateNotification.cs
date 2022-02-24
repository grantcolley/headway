using System;
using System.Threading.Tasks;

namespace Headway.Core.Notifications
{
    public interface IStateNotification
    {
        void Deregister(string target);
        void Register(string target, Action action);
        void Register(string target, Func<object, Task> functiona);
        void NotifyStateHasChanged(string target);
        Task NotifyStateHasChangedAsync(string target, object parameter);
    }
}
