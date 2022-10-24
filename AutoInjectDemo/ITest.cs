using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qin.AutoInject;

namespace AutoInjectDemo
{
    public interface ISingletonTest : ISingletonDependency { }

    public interface IScopedTest : IScopedDependency { }

    public interface ITransientTest : ITransientDependency { }  
    
    public interface ISingletonAttributeTest { }

    public interface IScopedAttributeTest { }

    public interface ITransientAttributeTest { }


}
