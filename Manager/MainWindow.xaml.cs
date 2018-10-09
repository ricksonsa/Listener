using Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio;
using NSpeex;
using DarrenLee.LiveStream.Audio;
using Microsoft.Win32;

namespace Manager
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DuplexChannelFactory<IListener> _channelFactory;
        public static IListener Server;
        public static Receiver receiver;

        public MainWindow()
        {
            InitializeComponent();

            string hostName = null;
            string winVer = null;
            string campaing = null;
            string audioVar = null;
            string loginSince = null;
            string windowsLockScreen = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Microsoft\\MOS\\BURAN"))
            {
                if (key != null)
                {
                    Object o = key.GetValue("USER_BURAN");//
                    if (o != null)
                    {
                        hostName = o.ToString();
                    }
                    o = key.GetValue("VERSAOWINDOWS_INSTALACAO");
                    if (o != null)
                    {
                        winVer = o.ToString();
                    }
                    o = key.GetValue("CAMPANHA");
                    if (o != null)
                    {
                        campaing = o.ToString();
                    }
                    o = key.GetValue("VARIACAO");
                    if (o != null)
                    {
                        audioVar = o.ToString();
                    }
                    o = key.GetValue("WINDOWS_LOGADO_HORARIO");
                    if (o != null)
                    {
                        loginSince = o.ToString();
                    }
                    o = key.GetValue("WINDOWS_LOGADO");
                    if (o != null)
                    {
                        windowsLockScreen = o.ToString();
                    }
                }
            }

            _channelFactory = new DuplexChannelFactory<IListener>(new ClientCallBack(),"ListenerServiceEndPoint");

            Server = _channelFactory.CreateChannel();

            Server.Login(Environment.UserName,hostName,campaing,winVer,loginSince,null,false);
        }

        public class ClientCallBack : IClient
        {
            public void DisconnectFromServer()
            {
                //throw new NotImplementedException();
            }

            public void GetUpdate(int value, string userName)
            {
                //throw new NotImplementedException();
            }

            public bool SendAudioToServer(string ip, int port, string requestName)
            {
                return true;
            }

            public void SendResponse()
            {
                Server.GetResponse(Environment.UserName);
            }
        }
    }
}
