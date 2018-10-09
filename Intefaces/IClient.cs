using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Intefaces
{
    public interface IClient
    {
        [OperationContract]
        void GetUpdate(int value, string userName);

        [OperationContract]
        void SendResponse();

        [OperationContract]
        bool SendAudioToServer(string ip, int port, string requestName);

        [OperationContract]
        void DisconnectFromServer();
    }
}
