using ArabyAds.Framework;
using System;
using System.IO;
using Topshelf;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                HostFactory.Run(x =>
                {
                    x.Service<AdFalconPortalMaintenanceJob>(s =>
                    {
                        s.ConstructUsing(name => new AdFalconPortalMaintenanceJob());
                        s.WhenStarted(tc =>
                        {
                            try
                            {
                                tc.Start();
                            }
                            catch
                            {
                                tc.Stop();
                            }
                        });

                        s.WhenStopped(tc => tc.Stop());
                    });

                    x.SetDisplayName("AdFalcon-PortalMaintenanceJob");
                    x.SetServiceName("AdFalcon-PortalMaintenanceJob");
                    x.RunAsLocalSystem();
                    x.StartAutomatically();
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.Error("General Exception", ex);
            }
        }
    }
}
