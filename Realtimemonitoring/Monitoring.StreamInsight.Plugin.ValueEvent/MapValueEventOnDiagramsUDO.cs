using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using Newtonsoft.Json;
using Sychev.Monitoring.StreamInsight.Contract.Observers;
using Sychev.Monitoring.Web.Contract.BL;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.StreamInsight.Plugin
{
    public class MapValueEventOnDiagramsUDO : CepOperator<StreamInsightServer.Models.ValueEvent, DiagramModelCollection>
    {
        private static List<DiagramModel> _diagrams;

        //преобразует входную коллекцию, полученную из окна, в выходную.
        public override IEnumerable<DiagramModelCollection> GenerateOutput(IEnumerable<StreamInsightServer.Models.ValueEvent> payloads)
        {
            if (_diagrams == null)
                _diagrams = DiagramRepository.GetDiagrams();


            //у каждого события, есть свое время, но для удобства отображения на диаграмме, укажим им время прихода на сервер.
            var eventsLocal = payloads.ToList();

            return eventsLocal
                .Select(variable => new DiagramModelCollection
                {
                    Data = JsonConvert.SerializeObject(Map(variable.Time, variable.Value), Formatting.Indented)
                })
                .ToList();
        }

        private static DiagramPointModel[] Map(DateTime currentTime, double value)
        {
            var data = new[]
            {
                new DiagramPointModel
                {
                    DiagramId = _diagrams[3].Id,
                    LineId = _diagrams[3].Lines[0].Id,
                    Y = value,
                    X = currentTime
                },
            };
            return data;
        }
    }
}