using System;
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
    }
}
