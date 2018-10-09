using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vysor
{
    public partial class Screens : Form
    {
        public Screens(string hostName,string userName)
        {
            InitializeComponent();
            Text = "Visualizando tela de " + userName;
            try
            {
                var url = "http://"+hostName+":88/telaonline.htm";
                webBrowser1.Url = new Uri(url);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro");
                this.Close();
            }
        }

        private void UrlUpdateTimer_Tick(object sender, EventArgs e)
        {
            if(webBrowser1.Url!=null)
                webBrowser1.Refresh();
        }
    }
}
