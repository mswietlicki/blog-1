using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using Newtonsoft.Json;
using Sychev.Monitoring.StreamInsight.Contract.Observers;
using Sychev.Monitoring.StreamInsightServer.Models;
using Sychev.Monitoring.Web.Contract.BL;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.StreamInsight.Plugin.ExceptionEvent
{
    public class MapExceptionEventOnDiagramsUDO : CepOperator<SerializableException, DiagramModelCollection>
    {
        private static List<DiagramModel> _diagrams;

        //преобразует входную коллекцию, полученную из окна, в выходную.
        public override IEnumerable<DiagramModelCollection> GenerateOutput(IEnumerable<SerializableException> payloads)
        {
            if (_diagrams == null)
                _diagrams = DiagramRepository.GetDiagrams();

            //у каждого события, есть свое время, но для удобства отображения на диаграмме, укажим им время прихода на сервер.
            var currentTime = DateTime.Now;
            var totalInstance = payloads.Count();

            //получаем из входного потока событие, количество удовлетворяющих нашему критерию.
            DiagramPointModel[] data = Map(totalInstance, currentTime);

            var dataStr = JsonConvert.SerializeObject(data, Formatting.Indented);

            return new List<DiagramModelCollection>
            {
                new DiagramModelCollection
                {
                    Data = dataStr
                }
            };
        }
        private static DiagramPointModel[] Map(int totalInstance, DateTime currentTime)
        {
            var data = new[]
            {
                new DiagramPointModel
                {
                    DiagramId = _diagrams[2].Id,
                    LineId = _diagrams[2].Lines[0].Id,
                    Y = totalInstance,
                    X = currentTime
                },
            };
            return data;
        }
    }
}