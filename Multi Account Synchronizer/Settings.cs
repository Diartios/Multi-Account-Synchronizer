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
        public Settings(List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>> apis)
        {
            InitializeComponent();
            this.apis = apis;
        }
        List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>> apis = new List<Tuple<PhoenixApi, JsonHandler, Player, Scene, Bot, BotForm>>();

        private void UpdateNumerics()
        {
            var api = apis.FirstOrDefault();
            if (api == null)
                return;
            MinilandInviteMin.Value = api.Item5.AcceptInviteDelay.Item1;
            MinilandInviteMax.Minimum = MinilandInviteMin.Value;
            MinilandInviteMax.Value = api.Item5.AcceptInviteDelay.Item2;
            MinilandInviteMin.Maximum = MinilandInviteMax.Value;

            AttackLureMin.Value = api.Item5.StartAttackDelay.Item1;
            AttackLureMax.Minimum = AttackLureMin.Value;
            AttackLureMax.Value = api.Item5.StartAttackDelay.Item2;
            AttackLureMin.Maximum = AttackLureMax.Value;

            ExitMinilandMin.Value = api.Item5.MinilandExitDelay.Item1;
            ExitMinilandMax.Minimum = ExitMinilandMin.Value;
            ExitMinilandMax.Value = api.Item5.MinilandExitDelay.Item2;
            ExitMinilandMin.Maximum = ExitMinilandMax.Value;

            UseAmuletMin.Value = api.Item5.AmuletUseDelay.Item1;
            UseAmuletMax.Minimum = UseAmuletMin.Value;
            UseAmuletMax.Value = api.Item5.AmuletUseDelay.Item2;

            DelayAfterKillMin.Value = api.Item5.DelayAfterKillPoint.Item1;
            DelayAfterKillMax.Value = api.Item5.DelayAfterKillPoint.Item2;

            if (api.Item5.InviteCommand != "")
                InviteCommandsComboBox.SelectedItem = api.Item5.InviteCommand;
            VokeDelay.Value = api.Item5.VokeDelay;
            MinMonsterCountVoke.Value = api.Item5.MinVokeMonsterCount;
            TrashItemsCheckBox.Checked = api.Item5.TrashItems;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            UpdateNumerics();

        }

        private void MinilandInviteMin_ValueChanged(object sender, EventArgs e)
        {
            MinilandInviteMax.Minimum = MinilandInviteMin.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)MinilandInviteMin.Value), ((int)MinilandInviteMax.Value));
            apis.ForEach(x => x.Item5.AcceptInviteDelay = delay);
        }

        private void MinilandInviteMax_ValueChanged(object sender, EventArgs e)
        {
            MinilandInviteMin.Maximum = MinilandInviteMax.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)MinilandInviteMin.Value), ((int)MinilandInviteMax.Value));
            apis.ForEach(x => x.Item5.AcceptInviteDelay = delay);
        }

        private void ExitMinilandMin_ValueChanged(object sender, EventArgs e)
        {
            ExitMinilandMax.Minimum = ExitMinilandMin.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)ExitMinilandMin.Value), ((int)ExitMinilandMax.Value));
            apis.ForEach(x => x.Item5.MinilandExitDelay = delay);
        }

        private void ExitMinilandMax_ValueChanged(object sender, EventArgs e)
        {
            ExitMinilandMin.Maximum = ExitMinilandMax.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)ExitMinilandMin.Value), ((int)ExitMinilandMax.Value));
            apis.ForEach(x => x.Item5.MinilandExitDelay = delay);
        }

        private void UseAmuletMin_ValueChanged(object sender, EventArgs e)
        {
            UseAmuletMax.Minimum = UseAmuletMin.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value));
            apis.ForEach(x => x.Item5.AmuletUseDelay = delay);
        }

        private void UseAmuletMax_ValueChanged(object sender, EventArgs e)
        {
            UseAmuletMin.Maximum = UseAmuletMax.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value));
            apis.ForEach(x => x.Item5.AmuletUseDelay = delay);
        }

        private void AttackLureMin_ValueChanged(object sender, EventArgs e)
        {
            AttackLureMax.Minimum = AttackLureMin.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value));
            apis.ForEach(x => x.Item5.AmuletUseDelay = delay);

        }

        private void AttackLureMax_ValueChanged(object sender, EventArgs e)
        {
            AttackLureMax.Minimum = AttackLureMin.Value;
            Tuple<int, int> delay = new Tuple<int, int>(((int)UseAmuletMin.Value), ((int)UseAmuletMax.Value));
            apis.ForEach(x => x.Item5.AmuletUseDelay = delay);
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
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
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int,int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }

        private void DelayAfterKillMax_ValueChanged(object sender, EventArgs e)
        {
            if (!DelayAfterKillMax.Enabled)
                return;
            DelayAfterKillMin.Maximum = DelayAfterKillMax.Value;
            apis.ForEach(x => x.Item5.DelayAfterKillPoint = new Tuple<int, int>(((int)DelayAfterKillMin.Value), ((int)DelayAfterKillMax.Value)));
        }
    }
}
