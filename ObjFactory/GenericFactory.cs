using System;
using System.Reflection;

namespace ObjFactory
{
    public class GenericFactory : CachedFactory<MethodInfo>
    {
        public GenericFactory()
            : base(type => typeof(GenericFactory).GetMethod("Create", new Type[] { }).MakeGenericMethod(type))
        {
        }

        public static T Create<T>() where T : new()
        {
            return new T();
        }

        public override object Create(Type type)
        {
            return GetFactoryFor(type).Invoke(null, null);
        }
    }
}
