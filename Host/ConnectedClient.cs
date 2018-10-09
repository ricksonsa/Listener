using Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    public class ConnectedClient
    {
        public IClient connection;
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Campaing { get; set; }
        public string WinVer { get; set; }
        public string LoginSince { get; set; }
        public string IP { get; set; }
        public string HostName { get; set; }
        public bool Connected { get; set; }
        public bool Supervisor { get; set; }
    }
}
