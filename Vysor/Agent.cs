using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vysor
{
    public class Agent
    {
        public string Name { get; set; }

        public Panel CreateAgent(string userName, string hostName)
        {
            Name = userName;

            Panel AgentPanel = new Panel
            {
                Name = userName,
                BackColor = Color.Transparent,
                Location = new Point(3, 3),
                Size = new Size(185, 50),
                BorderStyle = BorderStyle.FixedSingle
            };
            Label UsernameLbl = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Arial", 11, FontStyle.Regular),
                Text = userName,
            };
            Label HostnameLbl = new Label
            {
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 10, FontStyle.Regular),
                Text = hostName,
            };
            UsernameLbl.Click += new EventHandler((sender, e) => UsernameLbl_Click(sender, e, userName));
            HostnameLbl.Click += new EventHandler((sender, e) => UsernameLbl_Click(sender, e, userName));
            AgentPanel.Controls.Add(UsernameLbl);
            AgentPanel.Controls.Add(HostnameLbl);
            AgentPanel.Click += new EventHandler((sender, e) => UsernameLbl_Click(sender, e, userName));
            return AgentPanel;
        }

        private void UsernameLbl_Click(object sender, EventArgs e, string userName)
        {
            Vysor.GetInfo(userName);
        }
    }
}
