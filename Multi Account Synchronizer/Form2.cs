using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    public partial class Form2 : Form
    {
        public Form1 form1;
        PortFinder portfinder = new PortFinder();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
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

        private async void button3_Click(object sender, EventArgs e)
        {
            if (PortsComboBox.Text == "" || PortsComboBox.Text == null)
                return;
            List<string> zurna = portfinder.find_ports();
            bool zort;
            if (PortsComboBox.Text == "All")
            {
                zort = portfinder.find_ports().Count > 0;
                form1.IsConnected = zort;
            }
            else
            {
                string[] pirt = PortsComboBox.Text.Split(' ');
                string zart = PortsComboBox.Text;
                string nickname = "";
                if (pirt.Length > 1)
                {
                    zart = pirt[1];
                    nickname = pirt[0];
                }
                zort = form1.Connect(int.Parse(zart), nickname);
            }

            if (zort)
            {
                this.Visible = false;
                this.Close();
            }
        }
    }
}
