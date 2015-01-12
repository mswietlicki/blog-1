using System.ServiceModel;

namespace Sychev.Monitoring.StreamInsight.Contract.Observables
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, 
        InstanceContextMode = InstanceContextMode.Single)]
    public class WCFDataSource<T> : IWCFDataSource<T>
    {
        readonly WCFObservable<T> _obsInstance;

        public WCFDataSource(WCFObservable<T> obsInstance)
        {
            _obsInstance = obsInstance;
        }

        public void PushEvent(T value)
        {
            _obsInstance.OnNext(value);
        }
    }
}