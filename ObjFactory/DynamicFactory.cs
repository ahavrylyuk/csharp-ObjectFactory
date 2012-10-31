using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ObjFactory
{
    public class DynamicFactory : CachedFactory<Func<object>>
    {
        public DynamicFactory()
            : base(GenrateDynamicFactory)
        {
        }

        private static Func<object> GenrateDynamicFactory(Type type)
        {
            var dmethod = new DynamicMethod("", typeof(object), new[] { typeof(Type) });
            var igen = dmethod.GetILGenerator();
            var ci = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new CallingConventions(), Type.EmptyTypes, null);
            igen.Emit(OpCodes.Newobj, ci);
            igen.Emit(OpCodes.Castclass, type);
            igen.Emit(OpCodes.Ret);
            return (Func<object>)dmethod.CreateDelegate(typeof(Func<object>));
        }

        public override object Create(Type type)
        {
            return GetFactoryFor(type)();
        }
    }
}
