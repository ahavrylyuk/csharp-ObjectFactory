using System;
using System.Linq.Expressions;

namespace ObjFactory
{
    public class ExpressionFactory : CachedFactory<Func<object>>
    {
        public ExpressionFactory()
            : base(type => Expression.Lambda<Func<object>>(Expression.New(type)).Compile())
        {
        }

        public override object Create(Type type)
        {
            return GetFactoryFor(type)();
        }
    }
}
