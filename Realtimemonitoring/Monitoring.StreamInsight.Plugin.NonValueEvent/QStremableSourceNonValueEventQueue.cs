using System;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using Sychev.Monitoring.StreamInsight.Contract;
using Sychev.Monitoring.StreamInsight.Contract.Observables;
using Sychev.Monitoring.StreamInsight.Contract.Observers;

namespace Sychev.Monitoring.StreamInsight.Plugin.NonValueEvent
{
    public class QStremableSourceNonValueEventQueue : IQStremableSourcePlugin
    {
        public IQStreamable<DiagramModelCollection> Get(Application app, string wcfSourceUrl)
        {
            var observableAppFabricValueEventWcfSource = app.DefineObservable(() => new WCFObservable<StreamInsightServer.Models.NonValueEvent>(wcfSourceUrl + "AppFabricValueEventService", "AppFabricValueEventService"));

            //нельзя создать count окно меньше 2 events в окне. По этому мы делаем 2, и отсреливаем по 2.
            var appFabricEventValueQueue = from x in observableAppFabricValueEventWcfSource
                .ToPointStreamable(i => PointEvent.CreateInsert<StreamInsightServer.Models.NonValueEvent>(i.Time, i), AdvanceTimeSettings.IncreasingStartTime)
                .TumblingWindow(TimeSpan.FromMilliseconds(5000))
                select x.MapEventOnDiagrams();
            return appFabricEventValueQueue;
        }
    }
}