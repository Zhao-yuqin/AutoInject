using Qin.AutoInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInjectDemo
{
    public class SingletonTest : ISingletonTest
    {
        public SingletonTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }
    public class ScopedTest : IScopedTest
    {
        public ScopedTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }
    public class TransientTest : ITransientTest
    {
        public TransientTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }

    [AutoInject(InjectType.SingletonDependency,typeof(ISingletonAttributeTest))]
    public class SingletonAttributeTest : ISingletonAttributeTest
    {
        public SingletonAttributeTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }
    [AutoInject(InjectType.ScopedDependency, typeof(IScopedAttributeTest))]
    public class ScopedAttributeTest : IScopedAttributeTest
    {
        public ScopedAttributeTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }
    [AutoInject(InjectType.TransientDependency, typeof(ITransientAttributeTest))]
    public class TransientAttributeTest : ITransientAttributeTest
    {
        public TransientAttributeTest()
        {
            Console.WriteLine($"{this.GetType().Name}:{this.GetHashCode()}  has created");
        }

    }
}
