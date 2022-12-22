using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface ISetupFlowActions
    {
       void SetupActions(Dictionary<string, State>  stateDictionary);
    }
}
