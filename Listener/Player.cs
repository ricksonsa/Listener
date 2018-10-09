using Intefaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NSpeex;
using DarrenLee.LiveStream.Audio;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace Listener
{
    public partial class Player : Form
    {

        public static DuplexChannelFactory<IListener> _channelFactory;
        public static IListener Server;
        private static Sender audioSender;

        [Flags]
        public enum VConnectionState
        {
            Disconnected = 1,
            Connected = 2,
            Faulted = 3,
        }

        public static VConnectionState ServerConnectionState { get; set; }


        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        public Player()
        {
            InitializeComponent();

            ServerConnectionState = VConnectionState.Disconnected;      
        }

        private static void ConnectToServer()
        {
            string userName = Environment.UserName;
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

            _channelFactory = new DuplexChannelFactory<IListener>(new ClientCallBack(), "ListenerServiceEndPoint");
            Server = _channelFactory.CreateChannel();
            if(_channelFactory.State==CommunicationState.Created|| _channelFactory.State == CommunicationState.Opened)
            {
                ServerConnectionState = VConnectionState.Connected;
                
                while (true)
                {
                    int login = Server.Login(userName, hostName, campaing, winVer, loginSince, GetLocalIPAddress(), false);
                    if (login == 0)
                        break;
                }
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool StartRecording(string ip, int port, string resquestName)
        {
            DialogResult result = MessageBox.Show("O supervisor " + resquestName + " solicitou conexão a este computador. Deseja aceitar?","Monitoria",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            if (result.Equals(DialogResult.Yes))
            {
                audioSender = new Sender();
                bool uResult = audioSender.Send(ip, port);
                return uResult;
            }
            else
                return false;
        }

        public static void StopRecording()
        {
            audioSender.Disconnect();
        }

        internal static void SendResponse()
        {
            Server.GetResponse(Environment.UserName);        
        }

        private void Player_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.Enabled = false;
            IntPtr hWnd = GetConsoleWindow();
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
        }

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Logout();
        }

        private void TryReconnectTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Server.GetCurrentUsers();
            }
            catch (Exception)
            {
                ServerConnectionState = VConnectionState.Faulted;
            }
            finally
            {
                if (ServerConnectionState != VConnectionState.Connected)
                {
                    try//Attempt to reconnect
                    {
                        ConnectToServer();
                    }
                    catch (CommunicationException)
                    {
                        ServerConnectionState = VConnectionState.Disconnected;
                    }

                }
            }
        }

        private void CloseCMSitem_Click(object sender, EventArgs e)
        {
            Server.Logout();
            this.Close();
        }
    }
}
