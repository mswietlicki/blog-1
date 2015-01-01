using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Sychev.Monitoring.TrackingParticipant
{
    public sealed class MonitoringBehaviorExtensionElement : BehaviorExtensionElement
	{
        [ConfigurationProperty("profileName", DefaultValue = null, IsKey = false, IsRequired = false)]
        public string ProfileName
        {
            get { return (string)this["profileName"]; }
            set { this["profileName"] = value; }
        }

	    public override Type BehaviorType
	    {
		    get
			{
			    return typeof (MonitoringTrackingBehavior);
		    }
	    }

	    protected override object CreateBehavior()
		{
            return new MonitoringTrackingBehavior(ProfileName);
	    }
    }
}
