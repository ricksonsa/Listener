using Intefaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, AutomaticSessionShutdown = false)]
    public class Listener : IListener
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName,string hostName,string campaing, string winVer, string loginSince,string ip, bool super)
        {
            //esta alguem logado com meu nome?
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() == userName.ToLower())
                {
                    //se sim
                    return 1;
                }
            }

            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();

            ConnectedClient newClient = new ConnectedClient
            {
                connection = establishedUserConnection,
                UserName = userName,
                HostName = hostName,
                LoginSince = loginSince,
                Campaing = campaing,
                Supervisor = super,
                IP = ip,
                WinVer = winVer,
                Connected = true
            };

            _connectedClients.TryAdd(userName, newClient);

            UpdateHelper(0, userName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} {1} se conectou", DateTime.Now.ToString(), newClient.UserName);
            Console.ResetColor();

            //se nao
            return 0;
        }

        public void Logout()
        {
            ConnectedClient client = GetMyClient();
            if (client != null)
            {
                _connectedClients.TryRemove(client.UserName, out ConnectedClient removedeClient);

                UpdateHelper(1, removedeClient.UserName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0} {1} se desconectou", DateTime.Now.ToString(), removedeClient.UserName);
                Console.ResetColor();
            }
        }

        public List<List<string>> GetCurrentUsers()
        {
            List<List<string>> listOfUsers = new List<List<string>>();
            foreach (var client in _connectedClients)
            {
                listOfUsers.Add(GetUserInfo(client.Value.UserName));
            }
            return listOfUsers;
        }

        public void SendRequestToAll()
        {
            foreach (var client in _connectedClients)
            {
                client.Value.Connected = false;

                try
                {
                    client.Value.connection.SendResponse();
                }
                catch (Exception)
                {
                    client.Value.Connected = false;
                }
                finally
                {
                    if (!client.Value.Connected)
                    {
                        _connectedClients.TryRemove(client.Key, out ConnectedClient removedeClient);
                        UpdateHelper(1, removedeClient.UserName);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("{0} {1} time out.", DateTime.Now.ToString(), removedeClient.UserName);
                        Logger.ServerLog(removedeClient.UserName + " timed out.");
                        Console.ResetColor();
                    }
                }
            }
        }

        private void UpdateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetUpdate(value, userName);
                }
            }
        }

        public void GetResponse(string userName)
        {
            ConnectedClient client = GetMyClient();
            if (client != null)
            {
                client.Connected = true;
            }
        }

        public ConnectedClient GetMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach (var client in _connectedClients)
            {
                if (client.Value.connection == establishedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }

        public bool ConnectToClient(string name, string ip, int port, string requestName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() == name.ToLower())
                {
                    var uResult = client.Value.connection.SendAudioToServer(ip, port, requestName);
                    return uResult;
                }
            }
            return false;
        }

        public List<string> GetUserInfo(string name)
        {
            List<string> UserInfo = new List<string>();

            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() == name.ToLower())
                {
                    UserInfo.Add(client.Value.UserName);
                    UserInfo.Add(client.Value.HostName);
                    UserInfo.Add(client.Value.Campaing);
                    UserInfo.Add(client.Value.WinVer);
                    UserInfo.Add(client.Value.LoginSince);
                    UserInfo.Add(client.Value.IP);
                }
            }
            return UserInfo;
        }

        public void DisconnectClient(string name)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() == name.ToLower())
                {
                    client.Value.connection.DisconnectFromServer();
                }
            }
        }

    }
}
