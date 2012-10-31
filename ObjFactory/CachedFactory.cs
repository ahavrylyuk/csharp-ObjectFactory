using System;
using System.Collections.Generic;

namespace ObjFactory
{
    public abstract class CachedFactory<TValue>
    {
        private readonly Dictionary<Type, TValue> cache = new Dictionary<Type, TValue>();

        private readonly Func<Type, TValue> factory;

        protected CachedFactory(Func<Type, TValue> factory)
        {
            this.factory = factory;
        }

        public TValue GetFactoryFor(Type type)
        {
            TValue method;
            if (!cache.TryGetValue(type, out method))
            {
                cache[type] = method = factory(type);
            }
            return method;
        }

        public abstract object Create(Type type);
    }
}
