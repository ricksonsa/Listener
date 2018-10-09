using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Host
{
    class Program
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        private static Thread checkUpdate;
        public static Listener _server;
        public static ServiceHost host;

        static void Main(string[] args)
        {
            /*IntPtr hWnd = GetConsoleWindow();
             if (hWnd != IntPtr.Zero)
             {
                 ShowWindow(hWnd, 0);
             }*/
            OpenHost();
            host.Faulted += Host_Faulted;    
        }

        private static void OpenHost()
        {
            _server = new Listener();
            checkUpdate = new Thread(SendRequestHelper);
            checkUpdate.Start();

            using (host = new ServiceHost(_server))
            {
                Console.WriteLine("State: " + host.State.ToString());
                Console.WriteLine("OpenTimeout: " + host.OpenTimeout.ToString());
                Console.WriteLine("SingletonInstance: " + host.SingletonInstance.ToString());
                Console.WriteLine("CloseTimeout: " + host.CloseTimeout);
                Console.WriteLine("ChannelDispatchers: " + host.ChannelDispatchers.Count);

                host.Closed += Host_Closed;
                host.Opened += Host_Opened;

                try
                {
                    host.Open();
                    Console.WriteLine("Servidor ligado...");
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.Read();
                    host.Close();
                }
                catch (CommunicationException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.Read();
                    host.Close();
                }
                while (true)
                {
                    IntPtr hWnd = GetConsoleWindow();
                    /* if (hWnd != IntPtr.Zero)
                     {
                         ShowWindow(hWnd, 0);
                     }*/

                    string cmd = Console.ReadLine();

                    switch (cmd)
                    {
                        case "Count ChannelDispatchers":
                            Console.WriteLine("ChannelDispatchers: " + host.ChannelDispatchers.Count);
                            break;
                        case "Close":
                            Console.WriteLine("Servidor desligado.");
                            host.Close();
                            break;
                        case "Hide":
                            if (hWnd != IntPtr.Zero)
                            {
                                ShowWindow(hWnd, 0);
                            }
                            break;
                    }
                }
            }
        }

        private static void Host_Faulted(object sender, EventArgs e)
        {
            OpenHost();
            Console.WriteLine("Server faulted and reopened");
            Logger.ServerLog("Server faulted and reopened");
        }

        private static void SendRequestHelper()
        {
            Thread.Sleep(70000);
            try
            {
                _server.SendRequestToAll();
            }
            catch (Exception ex)
            {
                Logger.ServerLog(ex.ToString());
            }
            finally
            {
                SendRequestHelper();//Recursiva
            }
        }

        private static void Host_Opened(object sender, EventArgs e)
        {
        }

        private static void Host_Closed(object sender, EventArgs e)
        {
        }

    }
}
