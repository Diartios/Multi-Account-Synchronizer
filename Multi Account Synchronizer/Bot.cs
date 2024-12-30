using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static Multi_Account_Synchronizer.Scene;

namespace Multi_Account_Synchronizer
{
    internal class Bot
    {
        public Player player = new Player();
        public Scene scene = new Scene();
        public PhoenixApi phoenixapi = new PhoenixApi();
        public Random random = new Random();
        public bool map_changed = false;
        public bool run = false;
        public int delayer = 1;
        public RichTextBox richTextBox1;

        
        public Stopwatch Minilandsw = new Stopwatch();
        public bool ResetMinilandsw = false;
        public List<Tuple<string, int>> Buffs = new List<Tuple<string, int>>();
        public bool Buffing = false;
        public bool StartBuff = false;
        public bool Invite = false;
        public bool WaitingForMiniland = false;
        public string OwnerName = "";
        public bool LeaveMiniland = false;

        public int MinilandInterval = 300;
        public int DelaySameKey = 200;
        public int DelayDifferentKey = 2000;
        public List<string> DPSAccounts = new List<string>();
        public bool MiniEnabled = false;

        public bool DPS = false;
        public bool Buffer = true;
        public bool MinilandOwner = false;

        public bool WillUseAmulet = false;
        public bool UsedAmulet = false;

        #region Some Lure Shit
        public bool Otter = false;
        public bool Panda = false;
        public bool UseVoke = false;
        public bool GoNextLure = false;
        public bool StartAttack = false;
        public int LureMob = -1;
        public bool ReachedKillPoint = false;
        public bool UpdateBuff = false;
        public bool EnterMini = false;
        public bool Finished = false;
        #endregion

        #region Walking Variables
        public List<WalkPoint> Path = new List<WalkPoint>();
        public bool WalkWithPets = true;
        public bool RandomizeWalk = false;
        public int RandomizeWalkValue = 0;
        public bool WaitRespawn = false;
        public bool UseAmulet = false;
        public double WalkDelay = 0;
        public double EndDelay = 0;
        public double AmuletDelay = 0;
        #endregion


        #region Attack Variables
        public int AttackSearchRadius = 10;
        public bool AttackBlacklist = true;
        public bool AttackWhitelist = false;
        public bool Priority = false;
        public List<int> MonsterList = new List<int>();
        public bool IgnoreTarget = false;
        public int IgnoreTargetValue = 10;
        public bool KeepDistance = false;
        public int KeepDistanceValue = 0;
        #endregion

        #region Loot Variables
        public int LootSearchRadius = 10;
        public bool LootBlacklist = true;
        public bool LootWhiteList = false;
        public bool MyItems = true;
        public bool GroupItems = false;
        public bool NeutralItems = false;
        public bool IgnoreItem = false;
        public int IgnoreTıme = 30;
        public bool IgnoreFlower = false;
        public List<int> LootList = new List<int>();
        #endregion

        #region Security Variables
        public bool SecurityEnabled = true;
        public bool MapStop = false;
        List<int> Maps = new List<int>();
        #endregion
        public Bot()
        {
            Run();
        }


        public void AddLog(string message, string header)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            string log = $"[{currentTime}][{header}]: {message}";
            richTextBox1.AppendText(log + "\n");
        }
        private async Task walk(int x, int y)
        {
            Stopwatch s = Stopwatch.StartNew();
            map_changed = false;
            AddLog($"Walking to {x} | {y}", "Walk");
            while ((player.x != x || player.y != y) && !map_changed)
            {
                phoenixapi.player_walk(x, y);
                phoenixapi.pets_walk(x, y);
                await Task.Delay(490);
            }

        }
        private async Task walk(WalkPoint point)
        {
            ReachedKillPoint = false;
            map_changed = false;
            int randx = 0;
            int randy = 0;
            if (RandomizeWalk)
            {
                randx = random.Next(-1 * RandomizeWalkValue, RandomizeWalkValue + 1);
                randy = random.Next(-1 * RandomizeWalkValue, RandomizeWalkValue + 1);
            }
            AddLog($"Walking to {point.X} | {point.Y}", "Walk");
            while ((player.x != point.X + randx || player.y != point.Y + randy) && !map_changed && run)
            {
                phoenixapi.player_walk(point.X + randx, point.Y + randy);
                if (WalkWithPets)
                    phoenixapi.pets_walk(point.X + randx, point.Y + randy);
                await Task.Delay(500);
            
            }
            int delay = Convert.ToInt32((WalkDelay * 1000));
            await Task.Delay(delay);
            if (point.Kill)
            {
                ReachedKillPoint = true;
                AddLog("Reached kill point", "Walk");
            }
        }
        public void Start()
        {
            phoenixapi.start_bot();
        }
        public void Stop()
        {
            ReachedKillPoint = false;
            Finished = false;
            UpdateBuff = false;
            EnterMini = false;
            WaitingForMiniland = false;
            phoenixapi.stop_bot();
            LureMob = -1;
        }
        public async Task Run()
        {

            while (true)
            {
                while (!run)
                    await Task.Delay(10);
               
                if (DPS)
                {
                    int i = 0;
                    while (i < Path.Count && run)
                    {
                        if (i == 0 && Minilandsw.Elapsed.TotalSeconds == 0)
                        {
                            UpdateBuff = true;
                            while (UpdateBuff && run)
                                await Task.Delay(100);
                            if (!run)
                            {
                                Stop();
                                continue;
                            }
                            if (EnterMini)
                            {
                                await MinilandDPS();
                            }
                            
                        }
                        Finished = false;
                        WillUseAmulet = false;
                        UsedAmulet = false;
                        WalkPoint p = Path[i];
                        await walk(p);
                        if (!run)
                        {
                            Stop();
                            continue;
                        }
                        if (p.Kill && run)
                        {
                            await KillLure();
                            ReachedKillPoint = false;
                            await Loot();
                            Finished = true;
                            while (!GoNextLure && run)
                                await Task.Delay(100);
                            if (!run)
                            {
                                Stop();
                                continue;
                            }    
                            UpdateBuff = true;
                            while (UpdateBuff && run)
                                await Task.Delay(100);
                            if (!run)
                            {
                                Stop();
                                continue;
                            }
                            if (EnterMini)
                            {
                                await MinilandDPS();
                            }
                        }
                        i++;
                    }
                    i = 0;
                    
                    int end = Convert.ToInt32(EndDelay * 1000);
                    int amu = Convert.ToInt32(AmuletDelay * 1000);
                    await Task.Delay(end);
                    if (!run)
                    {
                        Stop();
                        continue;
                    }
                    if (UseAmulet)
                    {
                        Finished = false;
                        await Task.Delay(amu);
                        await Amulet();
                    }
                }
                else if (MinilandOwner)
                {
                    await MinilandMaster();
                }
                else if (Buffer)
                {
                    await MinilandAccount();
                }
               
                await Task.Delay(10);
            }
        }
        private async Task Amulet()
        {
            if (!run)
            {
                Stop();
                return;
            }
            player.updated = false;
            phoenixapi.query_inventory();
            while (!player.updated)
                await Task.Delay(1);
            var item = player.Inventory.Where(x => x.vnum == 2071).FirstOrDefault();
            if (item == null)
            {
                AddLog($"Couldn't find item by vnum {2071}", "Amulet");
                return;
            }
            map_changed = false;
            AddLog("Trying to use amulet", "Amulet");
            phoenixapi.send_packet($"u_i 1 {player.id} 2 {item.pos} 0 0");
            await Task.Delay(1000);
            phoenixapi.send_packet($"#u_i^1^{player.id}^2^{item.pos}^1");
            await Task.Delay(7000);
            while (!map_changed)
            {
                
                if (scene.LastAttacks.Count > 0)
                {
                    AddLog("Killing the monsters that attacked to us", "Self Defence");
                    while (scene.LastAttacks.Count > 0)
                    {
                        Scene.Entities entity = scene.LastAttacks.Values.ElementAt(0);
                        if (!scene.EntityData.ContainsKey(entity.id) || (DateTime.Now - entity.LastAttack).TotalSeconds > 10.0)
                            scene.LastAttacks.Remove(entity.id);
                        else
                        {
                            while (scene.EntityData.ContainsKey(entity.id))
                            {
                                phoenixapi.attack_monster(entity.id);
                                await Task.Delay(200);
                            }
                        }
                    }
                    await Task.Delay(1000);
                }

                AddLog("Trying to use amulet", "Amulet");
                phoenixapi.send_packet($"u_i 1 {player.id} 2 {item.pos} 0 0");
                await Task.Delay(1000);
                if (!run)
                {
                    Stop();
                    return;
                }
                phoenixapi.send_packet($"#u_i^1^{player.id}^2^{item.pos}^1");
                await Task.Delay(7000);
                await Task.Delay(100);
            }
        }
        private async Task Loot()
        {
            if (!run)
            {
                Stop();
                return;
            }
            AddLog("Started looting", "Loot");
            
            bool ignoreflower = true;
            if (!IgnoreFlower)
            {
                ignoreflower = false;
            }
            else if (player.FlowerSW.Elapsed.TotalSeconds == 0 && !player.StrongFlower)
            {
                ignoreflower = false;
            }
            else if (player.FlowerSW.Elapsed.Minutes > 7 && player.NormalFlower)
            {
                ignoreflower = false;
            }
            else if (!player.NormalFlower && !player.StrongFlower)
            {
                ignoreflower = false;
            }
            var loot = scene.GetLoot(LootSearchRadius, LootBlacklist, LootWhiteList, ignoreflower, MyItems, GroupItems, NeutralItems, LootList);
            Stopwatch sw = Stopwatch.StartNew();
            map_changed = false;
            while (loot.id != -1 && run && !map_changed)
            {
                while (scene.LootData.ContainsKey(loot.id) && run)
                {
                    phoenixapi.pick_up(loot.id);
                    await Task.Delay(300);
                    if (sw.Elapsed.TotalSeconds >= IgnoreTıme && IgnoreItem && scene.LootData.ContainsKey(loot.id))
                    {
                        scene.LootData.Remove(loot.id);
                        sw.Restart();
                    }
                }
                
                loot = scene.GetLoot(LootSearchRadius, LootBlacklist, LootWhiteList, ignoreflower, MyItems, GroupItems, NeutralItems, LootList);
                sw.Restart();
                await Task.Delay(10);
            }
            if (!run)
            {
                Stop();
                return;
            }
        }
        private async Task KillLure()
        {
            if (!run)
            {
                Stop();
                return;
            }
            AddLog("Killing lure", "Kill Lure");
            map_changed = false;
            if (!StartAttack)
                AddLog("Waiting other accounts to raech kill point", "Kill Lure");
            while (!StartAttack && !map_changed && run)
                await Task.Delay(100);
            if (!run)
            {
                Stop();
                return;
            }
            if (LureMob <= 0)
                AddLog("Waiting monster id", "Kill Lure");
            while (LureMob <= 0 && !map_changed && run)
                await Task.Delay(100);
            if (!run)
            {
                Stop();
                return;
            }
            if (map_changed)
                return;
            await Task.Delay(random.Next(500, 1250));
            Stopwatch luresw = Stopwatch.StartNew();
            Stopwatch targetsw = Stopwatch.StartNew();
            int oldmob = LureMob;
            while (LureMob > 0 && !map_changed && run)
            {
                phoenixapi.attack_monster(LureMob);
                await Task.Delay(200);
                if (oldmob != LureMob)
                {
                    oldmob = LureMob;
                    targetsw.Restart();
                }
                if (oldmob == LureMob && IgnoreTarget && targetsw.Elapsed.TotalSeconds >= IgnoreTargetValue)
                {
                    if (scene.EntityData.ContainsKey(LureMob))
                        scene.EntityData.Remove(LureMob);
                }
                if (UseVoke && luresw.Elapsed.TotalSeconds >= 3)
                {
                    if (Panda)
                    {
                        phoenixapi.use_pet_skill(LureMob, 1714);
                    }
                    if (Otter)
                    {
                        var mob = scene.EntityData.Values.Where(x => x.id == LureMob).FirstOrDefault();
                        if (mob != null)
                        {
                            phoenixapi.pets_walk(mob.Pos.X, mob.Pos.Y);
                            await Task.Delay(2000);
                            phoenixapi.use_pet_skill(LureMob, 663);
                        }
                    }
                }
            }
            AddLog("Finished killing lure", "Kill Lure");
            if (!run)
            {
                Stop();
                return;
            }
        }
        private async Task MinilandDPS()
        {
            if (!run)
            {
                Stop();
                return;
            }
            WaitingForMiniland = true;
            AddLog("Going to Miniland", "Miniland DPS");
            map_changed = false;
            while (player.map_id != 20001 && !map_changed && run)
                await Task.Delay(100);
            if (!run)
            {
                WaitingForMiniland = false;
                ResetMinilandsw = true;
                Stop();
                return;
            }
            if (player.map_id != 20001)
            {
                WaitingForMiniland = false;
                ResetMinilandsw = true;
                return;
            }
            WaitingForMiniland = false;
            AddLog("Entered Miniland", "Miniland DPS");
            await UseBuff();
            while (!StartBuff && run)
                await Task.Delay(100);
            if (!run)
            {
                ResetMinilandsw = true;
                Stop();
                return;
            }
            ResetMinilandsw = true;
            await Task.Delay(2000);
            while (!LeaveMiniland && run)
                await Task.Delay(100);
            if (!run)
            {
                Stop();
                return;
            }
            
            await LeaveMini();
        }
        private async Task MinilandAccount()
        {
            while (!StartBuff && run)
                await Task.Delay(100);
            if (!run)
                return;
            await UseBuff();
            await Task.Delay(30000);
        }
        private async Task MinilandMaster()
        {
            while (!Invite && run)
                await Task.Delay(100);
            if (!run)
                return;
            foreach (string account in DPSAccounts)
            {
                AddLog($"Inviting account with name {account}", "Miniland Master");
                phoenixapi.send_packet($"$Invite {account}");
                await Task.Delay(1000);
            }
            while (!StartBuff && run)
                await Task.Delay(100);
            if (!run)
                return;
            await UseBuff();
        }
        private async Task UseBuff()
        {
            if (!run)
                return;
            Buffing = true;
            foreach (var id in Buffs)
            {

                await Task.Delay(DelaySameKey * delayer);
                AddLog("Using " + id.Item1, "Buff");
                phoenixapi.send_packet($"u_s {id.Item2} 1 {player.id}");
                await Task.Delay(DelayDifferentKey);
            }
            Buffing = false;
        }
        private async Task LeaveMini()
        {
            //3 8

            int x = 3 + random.Next(-1,2);
            int y = 8 + random.Next(-1,2);
            Stopwatch sw = Stopwatch.StartNew();

            while (player.map_id == 20001)
            {
                phoenixapi.player_walk(x, y);
                phoenixapi.pets_walk(x, y);
                await Task.Delay(1500);
                if (sw.Elapsed.TotalSeconds >= 5)
                {
                    x = 3 + random.Next(-1, 2);
                    y = 8 + random.Next(-1, 2);
                    sw.Restart();
                }
            }
        }
        private async Task Main()
        {
            
        }



        public void ReadIni(string path)
        {
            if (!File.Exists(path))
                return;
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(path);
            #region Attack
            AttackSearchRadius = Statics.IniGetValueOrDefault(data, "Attack", "search_radius", 10);
            AttackBlacklist = Statics.IniGetValueOrDefault(data, "Attack", "blacklist", true);
            AttackWhitelist = Statics.IniGetValueOrDefault(data, "Attack", "whitelist", false);
            Priority = Statics.IniGetValueOrDefault(data, "Attack", "monster_priority", false);
            IgnoreTarget = Statics.IniGetValueOrDefault(data, "Attack", "ignore_target", false);
            IgnoreTargetValue = Statics.IniGetValueOrDefault(data, "Attack", "ignore_target_value", 10);
            KeepDistance = Statics.IniGetValueOrDefault(data, "Attack", "keep_distance", false);
            KeepDistanceValue = Statics.IniGetValueOrDefault(data, "Attack", "keep_distance_value", 0);
            int monstersize = Statics.IniGetValueOrDefault(data, "Attack", "monsters\\size", 0);
            MonsterList.Clear();
            for (int i = 1; i <= monstersize; i++)
            {
                int id = Statics.IniGetValueOrDefault(data, "Attack", $"monsters\\{i}\\id", -1);
                if (id == -1)
                    continue;
                MonsterList.Add(id);
            }
            #endregion

            #region Loot
            LootSearchRadius = Statics.IniGetValueOrDefault(data, "Loot", "search_radius", 10);
            LootBlacklist = Statics.IniGetValueOrDefault(data, "Loot", "blacklist", true);
            LootWhiteList = Statics.IniGetValueOrDefault(data, "Loot", "whitelist", false);
            IgnoreItem = Statics.IniGetValueOrDefault(data, "Loot", "ignore_item", false);
            IgnoreTıme = Statics.IniGetValueOrDefault(data, "Loot", "ignore_time", 30);
            IgnoreFlower = Statics.IniGetValueOrDefault(data, "Loot", "ignore_flowers", false);
            MyItems = Statics.IniGetValueOrDefault(data, "Loot", "pick_my_items", true);
            GroupItems = Statics.IniGetValueOrDefault(data, "Loot", "pick_group", false);
            NeutralItems = Statics.IniGetValueOrDefault(data, "Loot", "pick_neutral", false);
            int lootsize = Statics.IniGetValueOrDefault(data, "Loot", "loot_items\\size", 0);
            LootList.Clear();
            for (int i = 1; i <= lootsize; i++)
            {
                int id = Statics.IniGetValueOrDefault(data, "Loot", $"loot_items\\{i}\\id", -1);
                if (id == -1)
                    continue;
                LootList.Add(id);
            }
            #endregion

            #region Walking
            RandomizeWalk = Statics.IniGetValueOrDefault(data, "Walking", "randomize_walk", false);
            RandomizeWalkValue = Statics.IniGetValueOrDefault(data, "Walking", "randomize_walk_value", 0);
            WalkWithPets = Statics.IniGetValueOrDefault(data, "Walking", "walk_with_pets", true);
            WaitRespawn = Statics.IniGetValueOrDefault(data, "Walking", "wait_respawn", false);
            UseAmulet = Statics.IniGetValueOrDefault(data, "Walking", "use_amulet", false);
            WalkDelay = Statics.IniGetValueOrDefault(data, "Walking", "walk_delay", 0.0);
            EndDelay = Statics.IniGetValueOrDefault(data, "Walking", "end_delay", 0.0);
            AmuletDelay = Statics.IniGetValueOrDefault(data, "Walking", "amulet_delay", 0.0);
            int pathsize = Statics.IniGetValueOrDefault(data, "Walking", "path\\size", 0);
            Path.Clear();
            for (int i = 1; i <= pathsize; i++)
            {
                int x = Statics.IniGetValueOrDefault(data, "Walking", $"path\\{i}\\x", -1);
                int y = Statics.IniGetValueOrDefault(data, "Walking", $"path\\{i}\\y", -1);
                bool kill = Statics.IniGetValueOrDefault(data, "Walking", $"path\\{i}\\kill", false);
                if (x == -1 || y == -1)
                    continue;
                WalkPoint p = new WalkPoint(x, y, kill);
                Path.Add(p);
            }
            #endregion

            #region Miniland
            MiniEnabled = Statics.IniGetValueOrDefault(data, "Miniland", "enabled", true);
            MinilandInterval = Statics.IniGetValueOrDefault(data, "Miniland", "delay", 300);
            DelaySameKey = Statics.IniGetValueOrDefault(data, "Miniland", "same_key_delay", 200);
            DelayDifferentKey = Statics.IniGetValueOrDefault(data, "Miniland", "diff_key_delay", 2000);
            #endregion

            #region Security
            SecurityEnabled = Statics.IniGetValueOrDefault(data, "Security", "security_enabled", true);
            MapStop = Statics.IniGetValueOrDefault(data, "Security", "map_security\\stop", false);
            int mapssize = Statics.IniGetValueOrDefault(data, "Security", "map_security\\map_list\\size", 0);
            Maps.Clear();
            for (int i = 1; i <= mapssize; i++)
            {
                int id = Statics.IniGetValueOrDefault(data, "Security", $"map_security\\map_list\\{i}\\id", -1);
                if (id == -1)
                    continue;
                Maps.Add(id);
            }
            #endregion


        }

        public async void CreateNewIni(string path)
        {
            if (!File.Exists(path))
                return;
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(path);

            data["Attack"]["blacklist"] = "false";
            data["Attack"]["whitelist"] = "true";
            data["Attack"]["monsters\\size"] = "0";

            data["Loot"]["blacklist"] = "false";
            data["Loot"]["whitelist"] = "true";
            data["Loot"]["loot_items\\size"] = "0";

            data["Walking"]["path\\size"] = "0";

            data["Minland"]["enabled"] = "false";

            string newpath = path.Replace(".ini", "");
            newpath = $"{newpath} Sync.ini";
            if (File.Exists(newpath))
                File.Delete(newpath);
            File.WriteAllText(newpath, data.ToString());
            phoenixapi.load_settings(newpath);
            await Task.Delay(10 * 1000);
            File.Delete(newpath);
        }

        public void handle_packets(List<string> packet_splitted, string full_packet)
        {
            string header = packet_splitted[0];
            if (header == "at")
                handle_at(packet_splitted, full_packet);
            else if (header == "qnamli2")
                handle_qnamli2(packet_splitted, full_packet);
            else if (header == "c_map")
                handle_c_map(packet_splitted, full_packet);

        }

        public async void handle_qnamli2(List<string> packet_splitted, string full_packet)
        {
            if (full_packet.Contains("#mjoin") && full_packet.Contains(OwnerName))
            {
                await Task.Delay(random.Next(1000, 2001));
                phoenixapi.send_packet(packet_splitted[2]);
            }
                
        }

        private void handle_c_map(List<string> packet_splitted, string full_packet)
        {
            //c_map 0 2640 1
            if (packet_splitted.Count < 4)
                return;
            if (packet_splitted[3] != "1")
                return;
            int id = int.Parse(packet_splitted[2]);
            if (!Maps.Contains(id))
            {
                run = false;
                Stop();
            }
        }

        private void handle_at(List<string> packet_splitted, string full_packet)
        {
            map_changed = true;

        }
    }
}
