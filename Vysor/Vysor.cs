using DarrenLee.LiveStream.Audio;
using Intefaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vysor
{
    public partial class Vysor : Form
    {
        public static DuplexChannelFactory<IListener> _channelFactory;
        public static IListener Server;
        public static Receiver receiver;
        public static string ConnectedTo;
        public static string UserSelected;

        public static List<string> UserInfo = new List<string>();
        public static List<string> AgentList = new List<string>();

        public Vysor()
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<IListener>(new ClientCallBack(), "ListenerServiceEndPoint");
            Server = _channelFactory.CreateChannel();

            string hostName = null;
            string winVer = null;
            string campaing = null;
            string audioVar = null;
            string loginSince = null;
            string windowsLockScreen = null;
            ScreensLbl.Visible = false;

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

            UpdateListTimer.Start();
            UserInfoTimer.Start();

            //Server.Login(Environment.UserName, hostName, campaing, winVer, loginSince, true);
        }

        public static void GetInfo(string name)
        {
            ActiveForm.UseWaitCursor = true;
            UserInfo.Clear();
            UserInfo = Server.GetUserInfo(name);
            UserSelected = name;
        }

        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, AutomaticSessionShutdown = false, IncludeExceptionDetailInFaults = true)]
        public class ClientCallBack : IClient
        {
            public void DisconnectFromServer()
            {
                //Noth
            }

            public void GetUpdate(int value, string userName)
            {
                //throw new NotImplementedException();
            }

            public bool SendAudioToServer(string ip, int port, string requestName)
            {
                return false;
            }

            public void SendResponse()
            {
                Server.GetResponse(Environment.UserName);
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

        private void ConnectLbl_Click(object sender, EventArgs e)
        {


        }

        private void UserInfoTimer_Tick(object sender, EventArgs e)
        {
            if (UseWaitCursor)
                UseWaitCursor = false;

            if (UserInfo.Count > 0)
            {
                try
                {
                    var ping = new Ping();
                    var options = new PingOptions
                    {
                        DontFragment = true
                    };
                    string data = "Ping Test";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    var reply = ping.Send(UserInfo[5], 90, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        StatusLbl.Text = "Online";
                        StatusLbl.ForeColor = Color.Green;
                        StatusLbl.Visible = true;
                        UserNameLbl.Text = UserInfo[0];
                        UserNameLbl.Visible = true;
                        HostnameLbl.Text = UserInfo[1];
                        HostnameLbl.Visible = true;
                        CorporationLbl.Text = UserInfo[2];
                        CorporationLbl.Visible = false;
                        WindowsVersionLbl.Text = UserInfo[3];
                        WindowsVersionLbl.Visible = true;
                        LoginSinceLbl.Text = UserInfo[4];
                        LoginSinceLbl.Visible = true;

                    }
                    else
                    {
                        foreach (Control control in UserFlowPanel.Controls)
                        {
                            if (control.Name.Equals(UserInfo[0]))
                                control.BackColor = Color.LightGray;
                        }

                        StatusLbl.Text = "Offline";
                        StatusLbl.ForeColor = Color.Gray;

                        StatusLbl.Visible = true;

                        ConnectedToLbl.Visible = false;
                    }
                }
                catch (Exception)
                {
                    StatusLbl.Text = "Offline";
                    StatusLbl.ForeColor = Color.Gray;
                    StatusLbl.Visible = true;

                }

            }
        }

        private void UpdateListTimer_Tick(object sender, EventArgs e)
        {
            Agent agent = new Agent();
            List<List<string>> Users = new List<List<string>>();
            Users = Server.GetCurrentUsers();

            foreach (var user in Users)
            {
                if (!AgentList.Contains(user[0]))
                {

                    var userName = user[0];
                    var hostName = user[5];
                    var control = agent.CreateAgent(userName, hostName);
                    control.BackColor = Color.LightGreen;
                    UserFlowPanel.Controls.Add(control);
                    AgentList.Add(user[0]);

                }
            }

        }

        private void ScreensLbl_Click(object sender, EventArgs e)
        {
            if (UserInfo.Count > 0)
            {
                Screens screen;
                try
                {
                    screen = new Screens(UserInfo[1], UserInfo[0]);
                    screen.Show();
                }
                catch (Exception)
                {

                }
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if(UserSelected != null)
            {
                ConnectBtn.Enabled = false;
                ConnectedToLbl.Visible = true;
                if (ConnectBtn.Text == "Conectar")
                    ConnectedToLbl.Text = "Aguardando resposta de " + UserInfo[0] + "...";
                else
                    ConnectedToLbl.Visible = false;
                ScreensLbl.Visible = true;
                Thread tt = new Thread(t => ConnectToClient());
                tt.Start();
            }
            else
            {
                MessageBox.Show("Nenhum usuário selecionado.","Erro");
            }
            
        }

        private void ConnectToClient()
        {
            if (ConnectBtn.Text == "Conectar")
            {
                receiver = new Receiver();
                receiver.Receive(GetLocalIPAddress(), 13000);
                ScreensLbl.Visible = true;
                var uResult = Server.ConnectToClient(UserInfo[0], GetLocalIPAddress(), 13000, Environment.UserName);

                if (uResult)
                {
                    ConnectedTo = UserInfo[0];
                    ConnectedToLbl.Text = "Conectado a " + ConnectedTo;
                    ConnectedToLbl.Visible = true;
                    ConnectBtn.Text = "Desconectar";
                }
                else
                {
                    MessageBox.Show("Não conectou.");
                    ConnectedToLbl.Visible = false;
                }

            }
            else
            {
                Server.DisconnectClient(ConnectedTo);
                ScreensLbl.Visible = false;
                ConnectBtn.Text = "Conectar";
            }
            ConnectBtn.Enabled = true;
        }

        private void SearchRTB_TextChanged(object sender, EventArgs e)
        {
            foreach (Panel panel in UserFlowPanel.Controls)
            {
                panel.Visible = false;
                if (panel.Name.StartsWith(SearchRTB.Text))
                {
                    panel.Visible = true;
                }
            }
            if (string.IsNullOrWhiteSpace(SearchRTB.Text)|| SearchRTB.Text.Equals("Pesquisar..."))
            {
                foreach (Panel panel in UserFlowPanel.Controls)
                {
                    panel.Visible = true;

                }
            }
        }

        private void SearchRTB_Enter(object sender, EventArgs e)
        {
            if (SearchRTB.Text.Equals("Pesquisar..."))
                SearchRTB.Text = string.Empty;
        }

        private void SearchRTB_Leave(object sender, EventArgs e)
        {
            if (SearchRTB.Text.Equals(string.Empty))
                SearchRTB.Text = "Pesquisar...";
        }
    }
}
