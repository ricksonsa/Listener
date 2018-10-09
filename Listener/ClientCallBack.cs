using Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Listener
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, AutomaticSessionShutdown = false, IncludeExceptionDetailInFaults = true)]
    public class ClientCallBack : IClient
    {
        public void GetUpdate(int value, string userName)
        {
            //throw new NotImplementedException();
        }

        public bool SendAudioToServer(string ip, int port, string requestName)
        {
            return Player.StartRecording(ip, port, requestName);
        }

        public void SendResponse()
        {
            Player.SendResponse();
        }

        public void DisconnectFromServer()
        {
            Player.StopRecording();
        }
    }
}
