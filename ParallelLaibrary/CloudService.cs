using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.IO;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;

namespace ParallelLaibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    
    public partial class CloudService
    {
        public const string ServiceFolder = "Cloud";
    }
}
