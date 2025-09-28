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
        public Bot Bot;
        public Player player;
        public Scene scene;
        PhoenixApi api;
        public BotForm(Bot b, Player p, Scene s, PhoenixApi api)
        {
            Bot = b;
            player = p;
            this.api = api;
            scene = s;
            InitializeComponent();
            Bot.richTextBox1 = this.richTextBox1;
            Run();
        }

        private async void Run()
        {
            while (player.name == "")
                await Task.Delay(1);
            while (true)
            {
                this.Text = $"{player.name} | Working time: {Bot.WorkingTimeSw.Elapsed.ToString(@"hh\:mm\:ss")}";
                await Task.Delay(500);
            }
            
        }


        private async void BotForm_Load(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Bot.DPS = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Bot.Buffer = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Bot.MinilandOwner = radioButton3.Checked;
        }

        private async void roundedButton1_Click(object sender, EventArgs e)
        {
            UpdateComboBox();
        }
        private async void UpdateComboBox()
        {
            player.updated = false;
            
            api.query_skills_info();
            comboBox1.Items.Clear();
            while (!player.updated)
                await Task.Delay(1);
            foreach (var item in player.Skills)
            {
                comboBox1.Items.Add(item.Item1);
            }
            comboBox1.Items.Add("Partner Skill Q");
            comboBox1.Items.Add("Partner Skill W");
            comboBox1.Items.Add("Partner Skill E");
            comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;
        }
        private void roundedButton2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            if (list.Items.Contains(comboBox1.SelectedItem))
                return;
            list.Items.Add(comboBox1.SelectedItem);
            Bot.Buffs.Clear();
            Bot.PartnerBuffs.Clear();
            foreach (string item in list.Items)
            {
                if (item.Contains("Partner"))
                {
                    char skill = item[14];
                    switch (skill)
                    {
                        case 'Q':
                            Bot.PartnerBuffs.Add(0);
                            break;
                        case 'W':
                            Bot.PartnerBuffs.Add(1);
                            break;
                        case 'E':
                            Bot.PartnerBuffs.Add(2);
                            break;
                        default:
                            break;

                    }
                }
                else
                {
                    var id = player.Skills.Where(t => t.Item1 == item).FirstOrDefault();
                    Bot.Buffs.Add(id);
                }
                
            }
        }

        private async void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndex != -1)
            {
                list.Items.RemoveAt(list.SelectedIndex);

            }
            player.updated = false;
            api.query_skills_info();
            Bot.Buffs.Clear();
            Bot.PartnerBuffs.Clear();
            while (!player.updated)
                await Task.Delay(1);
            foreach (string item in list.Items)
            {
                if (item.Contains("Partner"))
                {
                    char skill = item[14];
                    switch (skill)
                    {
                        case 'Q':
                            Bot.PartnerBuffs.Add(0);
                            break;
                        case 'W':
                            Bot.PartnerBuffs.Add(1);
                            break;
                        case 'E':
                            Bot.PartnerBuffs.Add(2);
                            break;
                        default:
                            break;

                    }
                }
                else
                {
                    Tuple<string, int> tup = player.Skills.Where(t => t.Item1 == item).FirstOrDefault();
                    if (tup == null)
                        continue;
                    Bot.Buffs.Add(tup);
                }
               
            }
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Choose the phoenix bot profile that you use and want to sync", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Open file";
            o.Filter = "INI files|*.ini";
            DialogResult result = o.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            textBox2.Text = o.FileName;
            Bot.ReadIni(textBox2.Text);
            Bot.CreateNewIni(textBox2.Text);
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

            Bot.Otter = OttercheckBox.Checked;
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

            Bot.Panda = PandaCheckBox.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bot.DelayMultipler = (double)numericUpDown1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Bot.SwordsmanSP1 = checkBox1.Checked;
        }
    }
}
