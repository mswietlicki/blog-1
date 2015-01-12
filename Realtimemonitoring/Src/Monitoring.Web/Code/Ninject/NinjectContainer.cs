using System.Web.Mvc;
using Ninject;
using Ninject.Modules;

namespace Sychev.Monitoring.Web.Code.Ninject
{
    /// <summary>
    /// Its job is to Register Ninject Modules and Resolve Dependencies
    /// </summary>
    public class NinjectContainer
    {
        private static NinjectResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectResolver(modules);
            DependencyResolver.SetResolver(_resolver);
        }

        //Manually Resolve Dependencies
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}