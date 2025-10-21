using System;
using System.Collections.Generic;

namespace PhotonExacise.Scripts
{
    public static class Locator
    {
        public static void Register<T>(this T target)
        {
            _dict.TryAdd(typeof(T), target);
        }

        public static void Unregister<T>(this T target)
        {
            _dict.Remove(typeof(T));
        }

        public static T Get<T>() where T : class
        {
            if (_dict.TryGetValue(typeof(T), out object target))
            {
                return target as T;
            }

            return null;
        }

        private static Dictionary<Type, object> _dict = new();
    }
}
