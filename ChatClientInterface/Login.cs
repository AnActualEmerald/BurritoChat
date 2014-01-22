using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientInterface
{
    public partial class Login : Form
    {
        private ChatWindow c;

        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            c = new ChatWindow(name.Text, ip.Text, this);
            this.Visible = false;
        }

        private void clean(object sender, FormClosedEventArgs e)
        {
            try
            {
                c.ConfirmDisconnect();
            }catch(ObjectDisposedException){

            }
            Environment.Exit(0);
        }
    }
}
