using System;
using System.Activities.Tracking;
using System.Diagnostics;
using Sychev.AppFabricMonitoring.StreamInsightServer.Contract;
using Sychev.Monitoring.StreamInsight.ObservableServiceProxy.ValueEventProxy;

namespace Sychev.Monitoring.TrackingParticipant
{
    public sealed class MonitoringTrackingParticipant : System.Activities.Tracking.TrackingParticipant
    {
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            try
            {
                var connection = new WCFDataSourceOf_NonValueEventClient();

                if (record is System.Activities.Tracking.WorkflowInstanceRecord)
                {
                    var r = record as WorkflowInstanceRecord;
                    connection.PushEvent(new NonValueEvent
                    {
                        Name = r.ActivityDefinitionId,
                        Time = r.EventTime,
                        TrackEventType = (int) TrackEventType.InstanseStart
                    });

                }

                else if (record is System.Activities.Tracking.ActivityStateRecord)
                {
                    var r = record as ActivityStateRecord;
                    connection.PushEvent(new NonValueEvent
                    {
                        Name = r.Activity.Name,
                        Time = r.EventTime,
                        TrackEventType = (int) TrackEventType.ActivityStart
                    });
                }
            }
            catch(Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }
        }
    }
}