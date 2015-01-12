using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using Newtonsoft.Json;
using Sychev.AppFabricMonitoring.StreamInsightServer.Contract;
using Sychev.Monitoring.StreamInsight.Contract.Observers;
using Sychev.Monitoring.Web.Contract.BL;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.StreamInsight.Plugin.NonValueEvent
{
    public class MapEventOnDiagramsUDO : CepOperator<StreamInsightServer.Models.NonValueEvent, DiagramModelCollection>
    {
        private static List<DiagramModel> _diagrams;

        //преобразует входную коллекцию, полученную из окна, в выходную.
        public override IEnumerable<DiagramModelCollection> GenerateOutput(IEnumerable<StreamInsightServer.Models.NonValueEvent> payloads)
        {
            if (_diagrams == null)
                _diagrams = DiagramRepository.GetDiagrams();


            //у каждого событи€, есть свое врем€, но дл€ удобства отображени€ на диаграмме, укажим им врем€ прихода на сервер.
            var currentTime = DateTime.Now;
            var eventsLocal = payloads.ToList();

            //var dict = eventsLocal.GroupBy(i => i.GetType())
            //    .ToDictionary(i => i.Key, i => i.ToList());


            //получаем из входного потока событие, количество удовлетвор€ющих нашему критерию.
            var receiveNumber = eventsLocal.Sum(e => (e.Name == "Receive" && e.TrackEventType == (int)TrackEventType.ActivityStart) ? 1 : 0);
            var sendReplyToReceive = eventsLocal.Sum(e => (e.Name == "SendReplyToReceive" && e.TrackEventType == (int)TrackEventType.ActivityStart) ? 1 : 0);
            var totalInstance = eventsLocal.Sum(e => e.TrackEventType == (int)TrackEventType.InstanseStart ? 1 : 0);

            DiagramPointModel[] data = Map(totalInstance, currentTime, receiveNumber, sendReplyToReceive);
            //костыль. streamInsight не поддерживает массивы. ј без них совсем т€жко. ѕросто в SignalR клиенте, перед отправкой сделаем обратную десериализацию.
            //http://technet.microsoft.com/en-us/library/ee842720.aspx
            //http://technet.microsoft.com/en-us/library/ee378905.aspx
            //http://social.msdn.microsoft.com/Forums/en-US/07a702a5-2cf1-45d4-add7-572d81daeedd/can-streaminsight-handle-structured-eventsmessages?forum=streaminsight
            //http://social.msdn.microsoft.com/Forums/sqlserver/en-US/b16002dc-2c87-4f2c-acfd-d2d40b7ce787/simulating-collections-using-multiple-streams?forum=streaminsight

            var dataStr = JsonConvert.SerializeObject(data, Formatting.Indented);

            return new List<DiagramModelCollection>
            {
                new DiagramModelCollection
                {
                    Data = dataStr
                }
            };
        }

        private static DiagramPointModel[] Map(int totalInstance, DateTime currentTime, int receiveNumber, int sendReplyToReceive)
        {
            var data = new[]
            {
                new DiagramPointModel
                {
                    DiagramId = _diagrams[0].Id,
                    LineId = _diagrams[0].Lines[0].Id,
                    Y = totalInstance,
                    X = currentTime
                },
                new DiagramPointModel
                {
                    DiagramId = _diagrams[1].Id,
                    LineId = _diagrams[1].Lines[0].Id,
                    Y = receiveNumber,
                    X = currentTime
                },
                new DiagramPointModel
                {
                    DiagramId = _diagrams[1].Id,
                    LineId = _diagrams[1].Lines[1].Id,
                    Y = 0,
                    X = currentTime
                },
                new DiagramPointModel
                {
                    DiagramId = _diagrams[1].Id,
                    LineId = _diagrams[1].Lines[2].Id,
                    Y = sendReplyToReceive,
                    X = currentTime
                }
            };
            return data;
        }
    }
}