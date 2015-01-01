using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using Sychev.Monitoring.StreamInsight.Contract.Observers;

namespace Sychev.Monitoring.StreamInsight.Contract
{
    public interface IQStremableSourcePlugin
    {
        IQStreamable<DiagramModelCollection> Get(Application app, string wcfSourceUrl);
    }
}