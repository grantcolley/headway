using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Headway.Core.Mediators
{
    public class StateNotificationMediator : IStateNotificationMediator
    {
        private readonly Dictionary<string, Action> register;

        public StateNotificationMediator()
        {
            register = new Dictionary<string, Action>();
        }

        public void Register(string target, Action action)
        {
            if(register.ContainsKey(target))
            {
                return;
            }

            register.Add(target, action);
            Debug.WriteLine($"Registered {target}");
        }

        public void Deregister(string target)
        {
            if (register.ContainsKey(target))
            {
                register.Remove(target);
                Debug.WriteLine($"Deregistered {target}");
            }
        }

        public void NotifyStateHasChanged(string target)
        {
            if (register.ContainsKey(target))
            {
                register[target].Invoke();
            }
        }
    }
}
