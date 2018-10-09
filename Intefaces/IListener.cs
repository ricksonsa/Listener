using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Intefaces
{
    [ServiceContract(CallbackContract = typeof(IClient), SessionMode = SessionMode.Allowed)]
    public interface IListener
    {
        [OperationContract]
        void GetResponse(string userName);

        [OperationContract]
        int Login(string userName, string hostName, string campaing, string winVer, string loginSince,string ip, bool super);

        [OperationContract]
        void Logout();

        [OperationContract]
        bool ConnectToClient(string nome, string ip, int port, string requestName);

        [OperationContract]
        void DisconnectClient(string nome);

        [OperationContract]
        List<string> GetUserInfo(string name);

        [OperationContract]
        List<List<string>> GetCurrentUsers();
    }
}
