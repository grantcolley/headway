using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFlowStateActions
    {
       void SetupActions(Dictionary<string, State>  stateDictionary);
    }
}
