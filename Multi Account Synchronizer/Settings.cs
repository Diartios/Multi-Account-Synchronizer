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
    internal partial class Settings : Form
    {
        public Settings(List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>> apis, bool visible = true)
        {
            InitializeComponent();

            this.apis = apis;
            foreach (var x in this.Controls.OfType<TableLayoutPanel>())
            {
                foreach (var y in x.Controls.OfType<NumericUpDown>())
                {
                    y.Enabled = false;
                }
            }
            UpdateNumerics();
            foreach (var x in this.Controls.OfType<TableLayoutPanel>())
            {
                foreach (var y in x.Controls.OfType<NumericUpDown>())
                {
                    y.Enabled = true;
                }
            }
            if (!visible)
            {
                this.Visible = false;
                UpdateNumerics();
                this.Close();
            }

        }
        List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>> apis = new List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>>();



        private void UpdateNumerics()
        {
            var api = apis.FirstOrDefault();
            if (api == null)
                return;
            MinilandInviteMin.Value = api.Item5.AcceptInviteDelay.Item1;
            MinilandInviteMax.Value = api.Item5.AcceptInviteDelay.Item2;


            AttackLureMin.Value = api.Item5.StartAttackDelay.Item1;
            AttackLureMax.Value = api.Item5.StartAttackDelay.Item2;

            ExitMinilandMin.Value = api.Item5.MinilandExitDelay.Item1;
            ExitMinilandMax.Value = api.Item5.MinilandExitDelay.Item2;

            UseAmuletMin.Value = api.Item5.AmuletUseDelay.Item1;
            UseAmuletMax.Value = api.Item5.AmuletUseDelay.Item2;

            DelayAfterKillMin.Value = api.Item5.DelayAfterKillPoint.Item1;
            DelayAfterKillMax.Value = api.Item5.DelayAfterKillPoint.Item2;

            if (api.Item5.InviteCommand != "")
                InviteCommandsComboBox.SelectedItem = api.Item5.InviteCommand;
            VokeDelay.Value = api.Item5.VokeDelay;
            MinMonsterCountVoke.Value = api.Item5.MinVokeMonsterCount;
            StopAfterMinutes.Value = api.Item5.StopAfterMin;
            TrashItemsCheckBox.Checked = api.Item5.TrashItems;
            NormalFlower.Value = api.Item5.NormalFlowerUsage;
            StrongFlower.Value = api.Item5.StrongFlowerUsage;
            UpdateSettings();
        }
        private void UpdateSettings()
        {
            foreach (var x in apis)
            {
                x.Item5.AcceptInviteDelay = new Tuple<int, int>(((int)MinilandInviteMin.Value), ((int)MinilandInviteMax.Value));
                x.Item5.MinilandExitDelay = new Tuple<int, int>(((int)ExitMinilandMin.Value), ((int)ExitMinilandMax.Value));
                x.Item5.AmuletUseDelay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value));
                x.Item5.StartAttackDelay = new Tuple<int, int>(((int)AttackLureMin.Value), ((int)AttackLureMax.Value));
                x.Item5.InviteCommand = InviteCommandsComboBox.Text;
                x.Item5.VokeDelay = ((int)VokeDelay.Value);
                x.Item5.TrashItems = TrashItemsCheckBox.Checked;
                x.Item5.MinVokeMonsterCount = ((int)MinMonsterCountVoke.Value);
                x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value));
                x.Item5.StopAfterMin = ((int)StopAfterMinutes.Value);
                x.Item5.StrongFlowerUsage = ((int)StrongFlower.Value);
                x.Item5.NormalFlowerUsage = ((int)NormalFlower.Value);
            }

        }
        private void Settings_Load(object sender, EventArgs e)
        {
        }

        private void MinilandInviteMin_ValueChanged(object sender, EventArgs e)
        {
            if (!MinilandInviteMin.Enabled)
                return;
            MinilandInviteMax.Minimum = MinilandInviteMin.Value;
            apis.ForEach(x => x.Item5.AcceptInviteDelay = new Tuple<int, int>(((int)MinilandInviteMin.Value), ((int)MinilandInviteMax.Value)));
        }

        private void MinilandInviteMax_ValueChanged(object sender, EventArgs e)
        {
            if (!MinilandInviteMax.Enabled)
                return;
            MinilandInviteMin.Maximum = MinilandInviteMax.Value;
            apis.ForEach(x => x.Item5.AcceptInviteDelay = new Tuple<int, int>(((int)MinilandInviteMin.Value), ((int)MinilandInviteMax.Value)));
        }

        private void ExitMinilandMin_ValueChanged(object sender, EventArgs e)
        {
            if (!ExitMinilandMin.Enabled)
                return;
            ExitMinilandMax.Minimum = ExitMinilandMin.Value;
            apis.ForEach(x => x.Item5.MinilandExitDelay = new Tuple<int, int>(((int)ExitMinilandMin.Value), ((int)ExitMinilandMax.Value)));
        }

        private void ExitMinilandMax_ValueChanged(object sender, EventArgs e)
        {
            if (!ExitMinilandMax.Enabled)
                return;
            ExitMinilandMin.Maximum = ExitMinilandMax.Value;
            apis.ForEach(x => x.Item5.MinilandExitDelay = new Tuple<int, int>(((int)ExitMinilandMin.Value), ((int)ExitMinilandMax.Value)));
        }

        private void UseAmuletMin_ValueChanged(object sender, EventArgs e)
        {
            if (!UseAmuletMin.Enabled)
                return;
            UseAmuletMax.Minimum = UseAmuletMin.Value;
            apis.ForEach(x => x.Item5.AmuletUseDelay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value)));
        }

        private void UseAmuletMax_ValueChanged(object sender, EventArgs e)
        {
            if (!UseAmuletMax.Enabled)
                return;
            UseAmuletMin.Maximum = UseAmuletMax.Value;
            apis.ForEach(x => x.Item5.AmuletUseDelay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value)));
        }

        private void AttackLureMin_ValueChanged(object sender, EventArgs e)
        {
            if (!AttackLureMin.Enabled)
                return;
            AttackLureMax.Minimum = AttackLureMin.Value;
            apis.ForEach(x => x.Item5.StartAttackDelay = new Tuple<int, int>(((int)AttackLureMin.Value), ((int)AttackLureMax.Value)));

        }

        private void AttackLureMax_ValueChanged(object sender, EventArgs e)
        {
            if (!AttackLureMax.Enabled)
                return;
            AttackLureMin.Maximum = AttackLureMax.Value;
            apis.ForEach(x => x.Item5.StartAttackDelay = new Tuple<int, int>(((int)AttackLureMin.Value), ((int)AttackLureMax.Value)));
        }

        private void InviteCommandsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.InviteCommand = InviteCommandsComboBox.Text);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.VokeDelay = ((int)VokeDelay.Value));
        }

        private void TrashItemsCheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.TrashItems = TrashItemsCheckBox.Checked);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.MinVokeMonsterCount = ((int)MinMonsterCountVoke.Value));
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
            if (!DelayAfterKillMin.Enabled)
                return;
            DelayAfterKillMax.Minimum = DelayAfterKillMin.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void StopAfterMinutes_ValueChanged(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.StopAfterMin = ((int)StopAfterMinutes.Value));
        }

        private void numericUpDown2_ValueChanged_2(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.NormalFlowerUsage = ((int)NormalFlower.Value));
        }

        private void StrongFlower_ValueChanged(object sender, EventArgs e)
        {
            apis.ForEach(x => x.Item5.StrongFlowerUsage = ((int)StrongFlower.Value));
        }
    }
}
