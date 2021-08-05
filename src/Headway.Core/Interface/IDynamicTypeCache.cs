using Headway.Core.Dynamic;
using System;

namespace Headway.Core.Interface
{
    public interface IDynamicTypeCache
    {
        DynamicType GetDynamicType(string dynamicType, Type type);
    }
}
