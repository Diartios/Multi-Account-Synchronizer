using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Multi_Account_Synchronizer
{
    public partial class AddPortForm : Form
    {
        PortFinder portfinder = new PortFinder();
        PhoenixApi phoenixapi = new PhoenixApi();
        public string port;
        public string part;
        public bool IsConnected = false;
        public List<string> portList = new List<string>();
        public AddPortForm()
        {
            InitializeComponent();
        }

        private void AddPortForm_Load(object sender, EventArgs e)
        {
            List<string> zurna = portfinder.find_ports();
            PortsComboBox.Items.Clear();

            for (int i = 0; i < zurna.Count; i++)
            {
                string[] pirt = zurna[i].Split(' ');
                string zart = zurna[i];
                string nickname = "";
                if (pirt.Length > 1)
                {
                    zart = pirt[1];
                    nickname = pirt[0];
                }
                if (!portList.Contains(nickname))
                    PortsComboBox.Items.Add(zurna[i]);

            }
            if (PortsComboBox.Items.Count > 0)
                PortsComboBox.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (PortsComboBox.Text == "" || PortsComboBox.Text == null)
                return;
            string[] pirt = PortsComboBox.Text.Split(' ');
            string zart = PortsComboBox.Text;
            string nickname = "";
            if (pirt.Length > 1)
            {
                zart = pirt[1];
                nickname = pirt[0];
            }
            bool zort = Connect(int.Parse(zart), nickname);
            if (zort)
            {
                this.Visible = false;
                phoenixapi.Close();
                this.Close();
            }
        }
        public bool Connect(int port, string nickname)
        {
            IsConnected = phoenixapi.ConnectPort(port);
            part = port.ToString();
            this.port = port.ToString();
            if (nickname != "")
                part = nickname;
            return IsConnected;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> zurna = portfinder.find_ports();
            PortsComboBox.Items.Clear();
            for (int i = 0; i < zurna.Count; i++)
            {
                PortsComboBox.Items.Add(zurna[i]);

            }
            if (PortsComboBox.Items.Count > 0)
                PortsComboBox.SelectedIndex = 0;
        }
    }
}
