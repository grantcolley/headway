using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Headway.Core.Helpers
{
    public static class TypeHelper<T>
    {
        private static Func<T> creator =
            Expression.Lambda<Func<T>>(Expression.New(typeof(T).GetConstructor(Type.EmptyTypes)))
            .Compile();

        public static T Create()
        {
            return creator();
        }

        public static IEnumerable<T> CreateList()
        {
            var listType = typeof(List<>);
            var genericListType = listType.MakeGenericType(typeof(T));
            var instance = Activator.CreateInstance(genericListType);
            return (IEnumerable<T>)instance;
        }
    }
}
