using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Headway.Core.Helpers
{
    /// <summary>
    /// A dynamic type helper containing dynamic methods for the specified type.
    /// </summary>
    /// <typeparam name="T">The specified type.</typeparam>
    public class DynamicTypeHelper<T>
    {
        private readonly Dictionary<string, Func<T, object>> getters;
        private readonly Dictionary<string, Action<T, object>> setters;

        /// <summary>
        /// Initialises a new instance of the DynamicTypeHelper.
        /// </summary>
        /// <param name="createInstance">A dynamic method for creating new instance of the specified type.</param>
        /// <param name="getters">A dictionary of dynamic methods for property getters.</param>
        /// <param name="setters">A dictionary of dynamic methods for property setters.</param>
        /// <param name="supportedProperties">A list of property info's for supported properties.</param>
        public DynamicTypeHelper(Func<T> createInstance,
            Dictionary<string, Func<T, object>> getters,
            Dictionary<string, Action<T, object>> setters,
            IEnumerable<PropertyInfo> supportedProperties)
        {
            this.getters = getters;
            this.setters = setters;
            CreateInstance = createInstance;
            SupportedProperties = supportedProperties;
        }

        /// <summary>
        /// Gets a dynamic method for creating new instance of the specified type
        /// </summary>
        public Func<T> CreateInstance { get; private set; }

        /// <summary>
        /// Gets a list of property info's for supported properties.
        /// </summary>
        public IEnumerable<PropertyInfo> SupportedProperties { get; private set; }

        /// <summary>
        /// Gets a <see cref="PropertyInfo"/> from the <see cref="SupportedProperties"/> list.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The <see cref="PropertyInfo"/> matching the propertyName</returns>
        public PropertyInfo GetPropertyInfo(string propertyName)
        {
            return SupportedProperties.First(p => p.Name == propertyName);
        }

        /// <summary>
        /// A dynamic method for setting the value of the target property.
        /// </summary>
        /// <param name="target">The target property.</param>
        /// <param name="fieldName">The property name.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(T target, string fieldName, object value)
        {
            if (setters.ContainsKey(fieldName))
            {
                setters[fieldName](target, value);
                return;
            }

            throw new ArgumentOutOfRangeException(fieldName + " not supported.");
        }

        /// <summary>
        /// A dynamic method for getting the value of the target property.
        /// </summary>
        /// <param name="target">The target property.</param>
        /// <param name="fieldName">The property name.</param>
        /// <returns>The value of the property.</returns>
        public object GetValue(T target, string fieldName)
        {
            if (getters.ContainsKey(fieldName))
            {
                return getters[fieldName](target);
            }

            throw new ArgumentOutOfRangeException(fieldName + " not supported.");
        }
    }

    /// <summary>
    /// Builds a new instance of a <see cref="DynamicTypeHelper"/> for the specified type and caches it for re-use.
    /// </summary>
    public static class DynamicTypeHelper
    {
        private static readonly IDictionary<Type, object> cache = new Dictionary<Type, object>();

        private static int counter;

        private static object cacheLock = new object();

        /// <summary>
        /// Gets an instance of a <see cref="DynamicTypeHelper"/> for the specified type.
        /// Once created it is cached for re-use.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns>An instance of a <see cref="DynamicTypeHelper"/> for the specified type.</returns>
        public static DynamicTypeHelper<T> Get<T>() where T : class, new()
        {
            lock (cacheLock)
            {
                var t = typeof(T);

                if (cache.TryGetValue(t, out object result))
                {
                    return (DynamicTypeHelper<T>)result;
                }

                var propertyInfos = PropertyInfoHelper.GetPropertyInfos(t);
                var typeHelper = CreateTypeHelper<T>(propertyInfos);

                cache.Add(t, typeHelper);
                return typeHelper;
            }
        }

        private static DynamicTypeHelper<T> CreateTypeHelper<T>(IEnumerable<PropertyInfo> propertyInfos) where T : class, new()
        {
            var capacity = propertyInfos.Count() - 1;
            var getters = new Dictionary<string, Func<T, object>>(capacity);
            var setters = new Dictionary<string, Action<T, object>>(capacity);

            var createInstance = CreateInstance<T>();

            foreach (var propertyInfo in propertyInfos)
            {
                getters.Add(propertyInfo.Name, GetValue<T>(propertyInfo));

                if (propertyInfo.CanWrite)
                {
                    setters.Add(propertyInfo.Name, SetValue<T>(propertyInfo));
                }
            }

            return new DynamicTypeHelper<T>(createInstance, getters, setters, propertyInfos);
        }

        private static Func<T> CreateInstance<T>() where T : class, new()
        {
            var t = typeof(T);
            var methodName = "CreateInstance_" + typeof(T).Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, t, null, typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);
            return (Func<T>)dynMethod.CreateDelegate(typeof(Func<T>));
        }

        private static Func<T, object> GetValue<T>(PropertyInfo propertyInfo)
        {
            var getAccessor = propertyInfo.GetGetMethod();
            var methodName = "GetValue_" + propertyInfo.Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, typeof(T), new Type[] { typeof(object) },
                typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, getAccessor, null);
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<T, object>)dynMethod.CreateDelegate(typeof(Func<T, object>));
        }

        private static Action<T, object> SetValue<T>(PropertyInfo propertyInfo)
        {
            var setAccessor = propertyInfo.GetSetMethod();
            var methodName = "SetValue_" + propertyInfo.Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, typeof(void),
                new Type[] { typeof(T), typeof(object) }, typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }
            il.EmitCall(OpCodes.Callvirt, setAccessor, null);
            il.Emit(OpCodes.Ret);
            return (Action<T, object>)dynMethod.CreateDelegate(typeof(Action<T, object>));
        }

        private static int GetNextCounterValue()
        {
            return Interlocked.Increment(ref counter);
        }
    }
}
