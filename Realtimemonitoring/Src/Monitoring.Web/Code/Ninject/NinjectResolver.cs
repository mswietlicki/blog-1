using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;

namespace Sychev.Monitoring.Web.Code.Ninject
{
    /// <summary>
    /// Resolves Dependencies Using Ninject
    /// </summary>
    public class NinjectResolver : IDependencyResolver
    {
        public IKernel Kernel { get; private set; }
        public NinjectResolver(params NinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }
    }


    // List and Describe Necessary Modules
}