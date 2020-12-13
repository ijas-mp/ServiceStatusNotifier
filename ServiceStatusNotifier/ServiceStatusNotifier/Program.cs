using System;
using System.Diagnostics;
using System.Threading;
using System.Management;

namespace ServiceStatusNotifier
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC to exit program");
            Console.WriteLine("This application will list services which has chaged it's state");
            RunManagementEventWatcherForWindowsServices();
            do
            {
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(1000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
        private static void RunManagementEventWatcherForWindowsServices()
        {
            EventQuery eventQuery = new EventQuery();
            eventQuery.QueryString = "SELECT * FROM __InstanceModificationEvent within 2 WHERE targetinstance isa 'Win32_Service'";
            ManagementEventWatcher demoWatcher = new ManagementEventWatcher(eventQuery);
            demoWatcher.EventArrived += ServiceStatusChanged;
            demoWatcher.Start();
        }
        private static void ServiceStatusChanged(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            PropertyDataCollection props = targetInstance.Properties;
            Console.WriteLine("\nService status chaged");
            Console.WriteLine("Service Name {0}", props["Name"].Value.ToString());
            Console.WriteLine("Current status {0}", props["State"].Value.ToString());
        }
    }
}
