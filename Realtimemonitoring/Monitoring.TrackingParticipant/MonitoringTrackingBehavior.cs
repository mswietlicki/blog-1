using System.Activities.Tracking;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Activities.Tracking.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Sychev.Monitoring.TrackingParticipant
{
    public sealed class MonitoringTrackingBehavior : IServiceBehavior
    {
        readonly string _profileName;

        public MonitoringTrackingBehavior(string profileName)
        {
            this._profileName = profileName;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // get the tracking profile
            TrackingProfile trackingProfile = GetProfile(this._profileName);

            // configure the custom tracking with the new tracking profile
            var participant = new MonitoringTrackingParticipant { TrackingProfile = trackingProfile };

            // add it to the extensions collection
            ((WorkflowServiceHost)serviceHostBase).WorkflowExtensions.Add(participant);
        }

        TrackingProfile GetProfile(string profileName)
        {
            var trackingSection = (TrackingSection)ConfigurationManager.GetSection("system.serviceModel/tracking");
            if (trackingSection == null)
                return null;

            var match = trackingSection.TrackingProfiles
                .Where(i => i.Name == profileName)
                .ToList();

            if (!match.Any())
                throw new ConfigurationErrorsException(string.Format("Could not find a profile with name '{0}'", profileName));
            return match.First();
        }
    }
}
