using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.ServiceModel.Web;

namespace ParallelHost
{
    
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ParallelLaibrary.CloudService));

            host.Open();
            Console.WriteLine("host started");
            Console.ReadLine();
            host.Close();

        }
    }
}
