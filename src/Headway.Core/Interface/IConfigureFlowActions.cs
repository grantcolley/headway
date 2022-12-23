using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IConfigureFlowActions
    {
       void ConfigureActions(Dictionary<string, State>  stateDictionary);
    }
}
