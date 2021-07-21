using Headway.Core.Model;
using System;

namespace Headway.Core.Interface
{
    public interface IDynamicTypeCache
    {
        DynamicType GetDynamicType(string dynamicType, Type type)
    }
}
