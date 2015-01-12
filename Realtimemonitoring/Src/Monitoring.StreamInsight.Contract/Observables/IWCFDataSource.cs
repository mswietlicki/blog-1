using System.ServiceModel;

namespace Sychev.Monitoring.StreamInsight.Contract.Observables
{
	[ServiceContract]
	public interface IWCFDataSource<T>
	{
		[OperationContract(IsOneWay = true)]
		void PushEvent(T value);
	}
}