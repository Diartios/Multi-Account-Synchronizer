using Newtonsoft.Json.Linq;
using Multi_Account_Synchronizer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace Multi_Account_Synchronizer
{
    public partial class Form1 : Form
    {
        string part;
        string realport;
        List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>> apis = new List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>>();
        PhoenixApi phoenixapi = new PhoenixApi();
        PortFinder portfinder = new PortFinder();
        public Form2 form2;
        public bool IsConnected = false;
        public Form1()
        {
            InitializeComponent();
        }
        public bool Connect(int port, string nickname)
        {
            IsConnected = phoenixapi.ConnectPort(port);
            part = port.ToString();
            realport = port.ToString();
            if (nickname != "")
                part = nickname;
            return IsConnected;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                form2.ShowDialog();
                List<string> zurna = portfinder.find_ports();
                if (!IsConnected)
                    this.Close();
                foreach (RoundedButton button in tableLayoutPanel2.Controls.OfType<RoundedButton>())
                {
                    button.Enabled = false;
                }
                if (IsConnected)
                {
                    
                    phoenixapi.Close();
                    await CreatePort(int.Parse(realport), part);
                }
                else
                    this.Close();
                timer1.Start();
                foreach (RoundedButton button in tableLayoutPanel2.Controls.OfType<RoundedButton>())
                {
                    button.Enabled = true;
                }
                CheckPorts();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                this.Close();
            }
        }
        private async Task CheckPorts()
        {
            if (!File.Exists("Members.json"))
                return;
            JObject Members = new JObject();
            using (StreamReader file = new StreamReader("Members.json"))
            {
                string json = file.ReadToEnd();
                Members = JObject.Parse(json);
            }
            List<string> members = new List<string>();
            var ports = portfinder.find_ports();
            foreach (var member in Members["Members"])
            {
                foreach (var port in ports)
                {
                    if (port.Split(' ').Length != 2)
                        continue;
                    if (port.Split(' ')[0] == member["name"].ToString())
                    {
                        members.Add(port);
                    }
                }
            }
            foreach (Label l in tableLayoutPanel1.Controls.OfType<Label>())
            {
                string s = members.Where(x => x.Contains(l.Text)).FirstOrDefault();
                if (s == null)
                    continue;
                members.Remove(s);
            }
            if (members.Count == 0)
                return;
            string accs = "";

            for (int i = 0; i < members.Count; i++)
            {
                string[] splitted_port = members[i].Split(' ');
                accs += splitted_port[0];
                if (i == members.Count - 1)
                    continue;
                accs += ", ";
            }
            var result = MessageBox.Show($"Accounts {accs} found in settings would you like to add these accounts?", "Multi Account Synchronizer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            foreach (string port in members)
            {
                int pord = int.Parse(port.Split(' ')[1]);
                string name = port.Split(' ')[0];
                await CreatePort(pord, name);
            }
        }

        private int FindLowestEmptyRowIndex(TableLayoutPanel tableLayoutPanel)
        {
            for (int row = tableLayoutPanel.RowStyles.Count - 1; row >= 0; row--)
            {
                bool rowIsEmpty = true;

                for (int col = 0; col < tableLayoutPanel.ColumnStyles.Count; col++)
                {
                    if (tableLayoutPanel.GetControlFromPosition(col, row) != null)
                    {
                        rowIsEmpty = false;
                        break;
                    }
                }

                if (rowIsEmpty)
                {
                    return row;
                }
            }

            return -1;
        }

        private void UpdateRows()
        {
            int ba = -1;
            foreach (RowStyle row in tableLayoutPanel1.RowStyles)
            {
                row.Height = 100.0f / tableLayoutPanel1.RowStyles.Count;
            }
            for (int i = 0; i < apis.Count; i++)
            {
                apis[i].Item5.delayer = i + 1;
            }
            List<Control> controls = new List<Control>();
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                int index = tableLayoutPanel1.GetRow(c);
                if (index >= tableLayoutPanel1.RowStyles.Count)
                {
                    Console.WriteLine(tableLayoutPanel1.RowStyles.Count);
                    controls.Add(c);
                }
            }
            if (controls.Count > 0)
                ba = FindLowestEmptyRowIndex(tableLayoutPanel1);
            foreach (Control c in controls)
            {

                if (c.GetType().Name == "Label")
                    tableLayoutPanel1.Controls.Add(c, 0, ba);
                else if (c.Text == "Settings")
                    tableLayoutPanel1.Controls.Add(c, 1, ba);
                else if (c.Text == "Remove account")
                    tableLayoutPanel1.Controls.Add(c, 2, ba);
            }
        }

        private async Task CreatePort(int port, string nickname)
        {
            int i = FindLowestEmptyRowIndex(tableLayoutPanel1);
            if (i == -1)
            {
                RowStyle r = new RowStyle();
                r.SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles.Add(r);
                //updaterows();
                i = FindLowestEmptyRowIndex(tableLayoutPanel1);
            }
            PhoenixApi api = new PhoenixApi();
            JsonHandler handler = new JsonHandler();
            Player player = new Player();
            Scene scene = new Scene();
            Bot bot = new Bot();
            api.ConnectPort(port);
            handler.phoenixapi = api;
            handler.player = player;
            handler.scene = scene;
            handler.bot = bot;
            scene.player = player;
            player.phoenixapi = api;
            bot.player = player;
            bot.scene = scene;
            bot.phoenixapi = api;
            api.receive_message();
            handler.start();
            api.query_player_info();
            BotForm b = new BotForm(bot, player, scene, api);

            Label l1 = new Label();
            l1.Text = nickname;
            l1.Name = nickname;
            l1.AutoSize = true;
            l1.Anchor = AnchorStyles.None;


            RoundedButton r1 = new RoundedButton();
            r1.Text = "Settings";
            r1.Name = $"Settings {nickname}";
            r1.Size = new Size(r1.Size.Width, 25);
            r1.MinimumSize = new Size(r1.Size.Width, 25);
            r1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            r1.Click += (sender, e) =>
            {
                b.ShowDialog();
            };

            RoundedButton r2 = new RoundedButton();
            r2.Text = "Remove account";
            r2.Name = $"Remove account {nickname}";
            r2.Size = new Size(r2.Size.Width, 25);
            r2.MinimumSize = new Size(r2.Size.Width, 25);
            r2.Anchor = AnchorStyles.Left | AnchorStyles.Right;


            Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm> tuple = new Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>(api, handler, player, scene, bot, b);
            r2.Click += (sender, e) =>
            {
                handler.run = false;
                api.run = false;
                api.Close();
                bot.run = false;
                int index = tableLayoutPanel1.GetRow(l1);
                tableLayoutPanel1.Controls.Remove(r1);
                tableLayoutPanel1.Controls.Remove(l1);
                tableLayoutPanel1.Controls.Remove(r2);
                tableLayoutPanel1.RowStyles.RemoveAt(index);

                apis.Remove(tuple);
                UpdateRows();
                api = null;
                handler = null;
                player = null;
                scene = null;
                bot = null;
            };

            tableLayoutPanel1.Controls.Add(l1, 0, i);
            tableLayoutPanel1.Controls.Add(r1, 1, i);
            tableLayoutPanel1.Controls.Add(r2, 2, i);
            apis.Add(tuple);
            UpdateRows();
            //tabControl1.TabPages.Add(tabpage);
            await UpdateRoles(bot, player, handler, scene, api, b);
        }
        private async Task UpdateRoles(Bot bot, Player player, JsonHandler jsonhandler, Scene scene, PhoenixApi api, BotForm b)
        {
            if (!File.Exists("Members.json"))
                return;

            while (player.id <= 0)
                await Task.Delay(100);
            bot.random = new Random(player.id);
            JObject Members = new JObject();
            Random random = new Random(player.id);
            RadioButton DPS = b.radioButton1;
            RadioButton Buffer = b.radioButton2;
            RadioButton MinilandOwner = b.radioButton3;
            string path = "";
            using (StreamReader file = new StreamReader("Members.json"))
            {
                string json = file.ReadToEnd();
                Members = JObject.Parse(json);
            }
            int invitemin = Statics.JsonGetValueOrDefault(Members["Delays"], "Accept Invite Min", 1000);
            int invitemax = Statics.JsonGetValueOrDefault(Members["Delays"], "Accept Invite Max", 2000);
            int exitmin = Statics.JsonGetValueOrDefault(Members["Delays"], "Exit Miniland Min", 750);
            int exitmax = Statics.JsonGetValueOrDefault(Members["Delays"], "Exit Miniland Max", 2000);
            int useamuletmin = Statics.JsonGetValueOrDefault(Members["Delays"], "Use Amulet Min", 750);
            int useamuletmax = Statics.JsonGetValueOrDefault(Members["Delays"], "Use Amulet Max", 1450);
            int attackluremin = Statics.JsonGetValueOrDefault(Members["Delays"], "Attack Lure Min", 1500);
            int attackluremax = Statics.JsonGetValueOrDefault(Members["Delays"], "Attack Lure Max", 1500);
            Tuple<int,int> invite = new Tuple<int,int>(invitemin, invitemax);
            Tuple<int, int> exit = new Tuple<int, int>(exitmin, exitmax);
            Tuple<int, int> amulet = new Tuple<int, int>(useamuletmin, useamuletmax);
            Tuple<int, int> attack = new Tuple<int, int>(attackluremin, attackluremax);
            bot.AcceptInviteDelay = invite;
            bot.MinilandExitDelay = exit;
            bot.AmuletUseDelay = amulet;
            bot.StartAttackDelay = attack;
            foreach (var member in Members["Members"])
            {

                if (((int)member["id"]) == player.id)
                {
                    DPS.Checked = Statics.JsonGetValueOrDefault(member, "DPS", false);
                    Buffer.Checked = Statics.JsonGetValueOrDefault(member, "Buffer", true);
                    MinilandOwner.Checked = Statics.JsonGetValueOrDefault(member, "Miniland Owner", false);
                    path = Statics.JsonGetValueOrDefault(member, "Path", "");
                    if (path != "")
                    {
                        b.textBox2.Text = path;
                        bot.ReadIni(path);
                        bot.CreateNewIni(path);
                    }
                    foreach (var item in member["buffs"])
                    {
                        Tuple<string, int> t = new Tuple<string, int>(item["name"].ToString(), ((int)item["id"]));
                        bot.Buffs.Add(t);
                        b.list.Items.Add(t.Item1);
                    }
                }
            }
        }



        private void roundedButton1_Click(object sender, EventArgs e)
        {
            apis.ForEach(x => {
                if (!x.Item5.run)
                    x.Item5.AddLog("Bot started", "Information");
                x.Item5.run = true;
                x.Item1.start_bot();
            });
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            apis.ForEach(x => {
                if (x.Item5.run)
                {
                    x.Item5.AddLog("Bot stopped", "Information");
                }
                x.Item5.run = false;

            });
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {
               

            AddPortForm addPortForm = new AddPortForm();
            foreach (Label l in tableLayoutPanel1.Controls.OfType<Label>())
            {
                addPortForm.portList.Add(l.Text);
            }
            addPortForm.ShowDialog();
            if (addPortForm.IsConnected)
            {
                CreatePort(int.Parse(addPortForm.port), addPortForm.part);
                addPortForm = null;
            }
        }

        private void roundedButton4_Click(object sender, EventArgs e)
        {
            if (apis.Count == 0)
            {
                MessageBox.Show("There isn't any account", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var a = apis.FirstOrDefault();
            JObject content = new JObject();
            JArray players = new JArray();
            JObject delays = new JObject();
            delays.Add("Accept Invite Min", a.Item5.AcceptInviteDelay.Item1);
            delays.Add("Accept Invite Max", a.Item5.AcceptInviteDelay.Item2);
            delays.Add("Exit Miniland Min", a.Item5.MinilandExitDelay.Item1);
            delays.Add("Exit Miniland Max", a.Item5.MinilandExitDelay.Item2);
            delays.Add("Use Amulet Min", a.Item5.AmuletUseDelay.Item1);
            delays.Add("Use Amulet Max", a.Item5.AmuletUseDelay.Item2);
            delays.Add("Attack Lure Min", a.Item5.StartAttackDelay.Item1);
            delays.Add("Attack Lure Max", a.Item5.StartAttackDelay.Item2);
            content["Delays"] = delays;
            foreach (var api in apis)
            {
                JArray buffs = new JArray();
                foreach (var item in api.Item5.Buffs)
                {
                    JObject i = new JObject();
                    int id = item.Item2;
                    string name = item.Item1;
                    i.Add("id", id);
                    i.Add("name", name);
                    buffs.Add(i);
                }
                JObject newItem = new JObject();
                newItem.Add("name", api.Item3.name);
                newItem.Add("id", api.Item3.id);
                newItem.Add("DPS", api.Item5.DPS);
                newItem.Add("Miniland Owner", api.Item5.MinilandOwner);
                newItem.Add("Buffer", api.Item5.Buffer);
                newItem.Add("Path", api.Item6.textBox2.Text);
                newItem["buffs"] = buffs;
                players.Add(newItem);
                content["Members"] = players;
            }
            File.WriteAllText("Members.json", content.ToString());
            MessageBox.Show("Settings saved!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (apis.Count <= 0)
                return;
            var MinilandOwner = apis.Where(x => x.Item5.MinilandOwner).FirstOrDefault();

            bool timetobuff = false;
            bool invite = false;
            bool leavemini = false;
            bool entermini = false;
            string minilandownernick = "";

            if (MinilandOwner != null)
            {
                int seedofpowercount = MinilandOwner.Item3.Inventory.Where(x => x.Vnum == 1012).Sum(x => x.Quantity);
                MinilandOwner.Item5.DPSAccounts = apis.Where(x => x.Item5.DPS).Select(x => x.Item3.name).ToList();
                timetobuff = apis.Count(x => x.Item3.map_id == 20001) == apis.Count();
                invite = apis.Count(x => x.Item5.WaitingForMiniland && x.Item5.DPS && x.Item5.MiniEnabled) == apis.Count(x => x.Item5.DPS);
                leavemini = apis.Count(x => x.Item5.Buffing) == 0;
                minilandownernick = MinilandOwner.Item3.name;
                entermini = apis.Count(x => x.Item5.Minilandsw.Elapsed.TotalSeconds >= x.Item5.MinilandInterval || x.Item5.Minilandsw.Elapsed.TotalSeconds == 0) > 0 && seedofpowercount >= apis.Count(x => x.Item5.DPS && x.Item5.MiniEnabled);
            }
            
            bool updatemini = apis.Count(x => x.Item5.UpdateBuff && x.Item5.MiniEnabled) == apis.Count(x => x.Item5.DPS && x.Item5.MiniEnabled);

            bool gonextlure = apis.Count(x => x.Item5.Finished && x.Item5.DPS) == apis.Count(x => x.Item5.DPS);
            bool startkill = apis.Count(x => x.Item5.DPS && x.Item5.ReachedKillPoint) == apis.Count(x => x.Item5.DPS);
            List<int> luremobs = apis.Where(x => x.Item5.DPS).Select(x => x.Item4.CenterMob(x.Item5.AttackSearchRadius,x.Item5.AttackBlacklist,x.Item5.AttackWhitelist,x.Item5.MonsterList, x.Item5.Priority)).ToList();
            bool reset = apis.Count(x => x.Item5.DPS && x.Item5.ResetMinilandsw && x.Item5.MiniEnabled) == apis.Count(x => x.Item5.DPS && x.Item5.MiniEnabled) && apis.Count(x => x.Item5.DPS && x.Item5.MiniEnabled) > 0; 
            int luremob = -1;
            if (luremobs.Count(x => x > 0) > 0)
                luremob = luremobs.Where(x => x > 0).FirstOrDefault();

            bool HelpAmulet = apis.Count(x => x.Item5.DPS && x.Item5.NeedHelp) > 0;
            List<int> defencemobs = apis.Where(x => x.Item5.DPS && x.Item5.NeedHelp).Select(x => x.Item5.HelpNeededMobId).ToList();
            int defencemob = -1;
            if (defencemobs.Count(x => x > 0) > 0)
                defencemob = defencemobs.Where(x => x > 0).FirstOrDefault();

            foreach ( var api in apis )
            {
                api.Item5.DefenceMob = defencemob;
                api.Item5.HelpAmulet = HelpAmulet;
                api.Item5.StartBuff = timetobuff;
                api.Item5.Invite = invite;
                api.Item5.GoNextLure = gonextlure;
                api.Item5.LeaveMiniland = leavemini;
                api.Item5.LureMob = luremob;
                api.Item5.StartAttack = startkill;
                api.Item5.OwnerName = minilandownernick;
                if (reset)
                {
                    api.Item5.Minilandsw.Restart();
                    api.Item5.ResetMinilandsw = false;
                }
                if (updatemini)
                {
                    api.Item5.EnterMini = entermini;
                    api.Item5.UpdateBuff = false;
                    
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tryingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var map = Statics.LoadMap(1);
            Console.WriteLine(map.ToString());
            
        }

        private void roundedButton5_Click(object sender, EventArgs e)
        {
            Settings s = new Settings(apis);
            s.ShowDialog();
        }
    }
}
