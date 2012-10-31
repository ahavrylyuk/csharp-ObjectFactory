using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace ObjFactory
{
    public class TypeToCreate
    {
        public string Value { get; set; }
    }

    public class Program
    {
        private const int Count = 10000;

        private static readonly Type type = typeof(TypeToCreate);
        private static readonly GenericFactory genericFactory = new GenericFactory();
        private static readonly ExpressionFactory expressionFactory = new ExpressionFactory();
        private static readonly DynamicFactory dynamicFactory = new DynamicFactory();

        public static void Main()
        {
            WarmUp();

            var banchmarks = new[]
                {
                    new KeyValuePair<string, TimeSpan>("New", Benchmark(CreateNew, Count)),
                    new KeyValuePair<string, TimeSpan>("Activator", Benchmark(CreateActivator, Count)),
                    new KeyValuePair<string, TimeSpan>("Generic Invoke", Benchmark(CreateGenericInvoke, Count)),
                    new KeyValuePair<string, TimeSpan>("Generic", Benchmark(CreateGeneric, Count)),
                    new KeyValuePair<string, TimeSpan>("Expression", Benchmark(CreateExpression, Count)),
                    new KeyValuePair<string, TimeSpan>("DynamicMethod", Benchmark(CreateDynamicMethod, Count)),
                };

            foreach (var banchmark in banchmarks.OrderBy(k => k.Value))
            {
                Console.WriteLine("{0}: {1}", banchmark.Key, banchmark.Value);
            }
        }

        private static void WarmUp()
        {
            genericFactory.GetFactoryFor(type);
            expressionFactory.GetFactoryFor(type);
            dynamicFactory.GetFactoryFor(type);
        }

        protected static void CreateDynamicMethod()
        {
            var a = dynamicFactory.Create(type);
        }

        private static void CreateNew()
        {
            new TypeToCreate();
        }

        private static void CreateExpression()
        {
            expressionFactory.Create(type);
        }

        protected static void CreateActivator()
        {
            Activator.CreateInstance(type);
        }

        public static void CreateGeneric()
        {
            GenericFactory.Create<TypeToCreate>();
        }

        protected static void CreateGenericInvoke()
        {
            genericFactory.Create(type);
        }

        public static TimeSpan Benchmark(Action action, int count)
        {
            var start = Stopwatch.StartNew();
            while (count-- > 0)
            {
                action();
            }
            start.Stop();
            return start.Elapsed;
        }
    }
}
