using Ninject.Modules;
using Sychev.Monitoring.Web.Code.Repository;
using Sychev.Monitoring.Web.Contract.BL;

namespace Sychev.Monitoring.Web.Code.Ninject
{
    public class NinjectModules
    {
        //Return Lists of Modules in the Application
        public static NinjectModule[] Modules
        {
            get
            {
                //Return Modules you want to use for DI
                return new[] { new MainModule() };
            }
        }

        //Main Module For Application. 
        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IPointsRepository>().To<PointsRepository>();
                Bind<IDiagramRepository>().To<DiagramRepository>();
            }
        }
    }
}