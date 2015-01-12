using Microsoft.ComplexEventProcessing.Linq;
using Sychev.Monitoring.StreamInsight.Contract.Observers;
using Sychev.Monitoring.StreamInsightServer.Models;

namespace Sychev.Monitoring.StreamInsight.Plugin.ExceptionEvent
{
    public static class MapExceptionEventOnDiagramsExtensions
    {
        //extension method которй за собой пр€чит CepOperator
        [CepUserDefinedOperator(typeof(MapExceptionEventOnDiagramsUDO))]
        public static DiagramModelCollection MapEventOnDiagrams(this CepWindow<SerializableException> window)
        {
            throw CepUtility.DoNotCall();
        }
    }
}