using Microsoft.ComplexEventProcessing.Linq;
using Sychev.Monitoring.StreamInsight.Contract.Observers;

namespace Sychev.Monitoring.StreamInsight.Plugin.NonValueEvent
{
    public static class MapEventOnDiagramsExtensions
    {
        //extension method которй за собой пр€чит CepOperator
        [CepUserDefinedOperator(typeof(MapEventOnDiagramsUDO))]
        public static DiagramModelCollection MapEventOnDiagrams(this CepWindow<StreamInsightServer.Models.NonValueEvent> window)
        {
            throw CepUtility.DoNotCall();
        }
    }
}