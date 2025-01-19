using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    internal partial class BotForm : Form
    {
        public Bot bot;
        public Player player;
        public Scene scene;
        PhoenixApi api;
        public BotForm(Bot b, Player p, Scene s, PhoenixApi api)
        {
            bot = b;
            player = p;
            this.api = api;
            scene = s;
            InitializeComponent();
            bot.richTextBox1 = this.richTextBox1;
        }


        private async void BotForm_Load(object sender, EventArgs e)
        {
            while (player.name == "")
                await Task.Delay(1);
            this.Text = $"Multi Account Synchronizer - {player.name}";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bot.DPS = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bot.Buffer = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            bot.MinilandOwner = radioButton3.Checked;
        }

        private async void roundedButton1_Click(object sender, EventArgs e)
        {
            api.query_skills_info();
            comboBox1.Items.Clear();
            player.updated = false;
            while (!player.updated)
                await Task.Delay(1);
            foreach (var item in player.Skills)
            {
                comboBox1.Items.Add(item.Item1);
            }
            comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            if (list.Items.Contains(comboBox1.SelectedItem))
                return;
            list.Items.Add(comboBox1.SelectedItem);
            bot.Buffs.Clear();
            foreach (string item in list.Items)
            {
                var id = player.Skills.Where(t => t.Item1 == item).FirstOrDefault();
                bot.Buffs.Add(id);
            }
        }

        private async void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndex != -1)
            {
                list.Items.RemoveAt(list.SelectedIndex);

            }
            api.query_skills_info();
            bot.Buffs.Clear();
            player.updated = false;
            while (!player.updated)
                await Task.Delay(1);
            foreach (string item in list.Items)
            {
                Tuple<string, int> tup = player.Skills.Where(t => t.Item1 == item).FirstOrDefault();
                if (tup == null)
                    continue;
                bot.Buffs.Add(tup);
            }
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Choose the phoenix bot profile that you use and want to sync", "INFO",MessageBoxButtons.OK,MessageBoxIcon.Information);
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Open file";
            o.Filter = "INI files|*.ini";
            DialogResult result = o.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            textBox2.Text = o.FileName;
            bot.ReadIni(textBox2.Text);
            bot.CreateNewIni(textBox2.Text);
            MessageBox.Show("The Phoenix Bot profile has changed with the edited one. You can make changes in attack, items, security but do not save your new Phoenix Bot settings", "CAUTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void OttercheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OttercheckBox.Checked)
            {
                if (PandaCheckBox.Checked)
                    PandaCheckBox.Checked = false;
                if (!player.Pet.Skills.ContainsKey(663))
                {
                    player.Pet.Skills.Clear();
                    player.Pet.Skills[663] = true;
                }
            }
                
            bot.Otter = OttercheckBox.Checked;
        }

        private void PandaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (PandaCheckBox.Checked)
            {
                if (OttercheckBox.Checked)
                    OttercheckBox.Checked = false;
                if (!player.Pet.Skills.ContainsKey(1714))
                {
                    player.Pet.Skills.Clear();
                    player.Pet.Skills[1714] = true;
                }
                
                
            }
                
            bot.Panda = PandaCheckBox.Checked;
        }
    }
}
