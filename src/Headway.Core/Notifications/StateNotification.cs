using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Notifications
{
    public class StateNotification : IStateNotification
    {
        private readonly Dictionary<string, Action> actions;
        private readonly Dictionary<string, Func<object, Task>> functions;

        public StateNotification()
        {
            actions = new Dictionary<string, Action>();
            functions = new Dictionary<string, Func<object, Task>>();
        }

        public void Register(string target, Func<object, Task> function)
        {
            if (functions.ContainsKey(target))
            {
                return;
            }

            functions.Add(target, function);
        }

        public void Register(string target, Action action)
        {
            if(actions.ContainsKey(target))
            {
                return;
            }

            actions.Add(target, action);
        }

        public void Deregister(string target)
        {
            if (actions.ContainsKey(target))
            {
                actions.Remove(target);
            }

            if (functions.ContainsKey(target))
            {
                functions.Remove(target);
            }
        }

        public void NotifyStateHasChanged(string target)
        {
            if (actions.ContainsKey(target))
            {
                actions[target].Invoke();
            }
        }

        public async Task NotifyStateHasChangedAsync(string target, object parameter)
        {
            if (functions.ContainsKey(target))
            {
                await functions[target].Invoke(parameter).ConfigureAwait(false);
            }
        }
    }
}
