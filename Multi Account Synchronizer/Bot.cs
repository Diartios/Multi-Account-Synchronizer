﻿using System;
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
        public Player Player = new Player();
        public Scene Scene = new Scene();
        public PhoenixApi Api = new PhoenixApi();
        public Random randomom = new Random();
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

        public bool NeedHelp = false;
        public bool HelpAmulet = false;
        public int HelpNeededMobId = -1;
        public int DefenceMob = -1;
        

        int[][] CurrentMap = Statics.LoadMap(-1);
        int LastLoadedMap = -1;
        int LastAttackedMob = -1;

        #region Some Lure Shit
        public bool Otter = false;
        public bool Panda = false;
        public bool UseVoke = false;
        public bool UpdateVoke = false;
        public int MinVokeMonsterCount = 6;
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
        private int CurrentLootId = -1;
        #endregion

        #region Security Variables
        public bool SecurityEnabled = true;
        public bool MapStop = false;
        List<int> Maps = new List<int>();
        #endregion

        #region Delay Variables
        public Tuple<int, int> MinilandExitDelay = new Tuple<int, int>(750, 2000);
        public Tuple<int, int> AmuletUseDelay = new Tuple<int, int>(750, 1450);
        public Tuple<int, int> StartAttackDelay = new Tuple<int, int>(1500, 2100);
        public Tuple<int, int> AcceptInviteDelay = new Tuple<int, int>(1000, 2000);
        public Tuple<int, int> DelayAfterKillPoint = new Tuple<int, int>(450, 850);
        public int VokeDelay = 750;
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
        private async Task Walk(int x, int y)
        {
            Stopwatch s = Stopwatch.StartNew();
            map_changed = false;
            AddLog($"Walking to {x} | {y}", "Walk");
            while ((Player.x != x || Player.y != y) && !map_changed)
            {
                Api.player_walk(x, y);
                Api.pets_walk(x, y);
                await Task.Delay(490);
            }

        }
        private async Task Walk(WalkPoint point)
        {
            if (LastLoadedMap != Player.map_id && Player.map_id != 20001)
            {
                CurrentMap = Statics.LoadMap(Player.map_id);
                LastLoadedMap = Player.map_id;
            }
            ReachedKillPoint = false;
            map_changed = false;
            int randx = 0;
            int randy = 0;
            if (RandomizeWalk && CurrentMap.Length > 0)
            {
                randx = random.Next(-1 * RandomizeWalkValue, RandomizeWalkValue + 1);
                randy = random.Next(-1 * RandomizeWalkValue, RandomizeWalkValue + 1);
            }
            //check if the cell is walkable
            if (CurrentMap.Length > 0 && RandomizeWalk)
            {
                if (point.X + randx > CurrentMap.Length)
                {
                    randx = 0;
                    randy = 0;
                }
                else if (point.Y + randy > CurrentMap[point.X + randx].Length)
                {
                    randx = 0;
                    randy = 0;
                }
                else if (CurrentMap[point.X + randx][point.Y + randy] == 0)
                {
                    randx = 0;
                    randy = 0;
                }
            }

            AddLog($"Walking to {point.X + randx} | {point.Y + randy}", "Walk");
            while ((Player.x != point.X + randx || Player.y != point.Y + randy) && !map_changed && run)
            {
                Api.player_walk(point.X + randx, point.Y + randy);
                if (WalkWithPets)
                    Api.pets_walk(point.X + randx, point.Y + randy);
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
            Api.start_bot();
        }
        public void Stop()
        {
            ReachedKillPoint = false;
            Finished = false;
            UpdateBuff = false;
            EnterMini = false;
            WaitingForMiniland = false;
            Api.stop_bot();
            LureMob = -1;
        }
        private int DelayGenerator(Tuple<int,int> range)
        {
            if (range.Item1 == 0 && range.Item2 == 0)
                return 0;
            else if (range.Item1 == range.Item2)
                return range.Item1;

            int delay = random.Next(range.Item1,range.Item2);
            return delay;
        }
        public async Task Run()
        {

            while (true)
            {
                await Task.Delay(10);
                while (!run)
                    await Task.Delay(10);
               
                if (DPS)
                {
                    int i = 0;
                    while (i < Path.Count && run)
                    {
                        if (i == 0 && Minilandsw.Elapsed.TotalSeconds == 0 && MiniEnabled || i == 0 && Minilandsw.Elapsed.TotalSeconds > MinilandInterval && MiniEnabled)
                        {
                            UpdateBuff = true;
                            while (UpdateBuff && run)
                                await Task.Delay(100);
                            if (!run)
                            {
                                Stop();
                                break;
                            }
                            if (EnterMini)
                            {
                                await MinilandDPS();
                            }
                            
                        }
                        //wait respawn
                        if (WaitRespawn && Multi_Account_Synchronizer.Scene.CenterMob(AttackSearchRadius, AttackBlacklist, AttackWhitelist, MonsterList, Priority) <= 0 && run && i == 0)
                        {
                            AddLog("Waiting for respawn", "Bot");
                        }
                        //wait respawn
                        if (WaitRespawn && Scene.CenterMob(AttackSearchRadius, AttackBlacklist, AttackWhitelist, MonsterList, Priority) <= 0 && run && i == 0)
                        {
                            AddLog("Waiting for respawn", "Bot");
                        }
                        while (WaitRespawn && Scene.CenterMob(AttackSearchRadius, AttackBlacklist, AttackWhitelist, MonsterList, Priority) <= 0 && run && i == 0)
                            await Task.Delay(100);
                        if (!run)
                        {
                            Stop();
                            break;
                        }
                        Finished = false;
                        WalkPoint p = Path[i];
                        await Walk(p);
                        if (!run)
                        {
                            Stop();
                            break;
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
                                break;
                            }
                            
                            if (MiniEnabled && i != Path.Count - 1)
                            {
                                UpdateBuff = true;
                                while (UpdateBuff && run)
                                    await Task.Delay(100);
                                if (!run)
                                {
                                    Stop();
                                    break;
                                }
                                if (EnterMini)
                                {
                                    await MinilandDPS();
                                }
                            }
                            if (i != Path.Count - 1)
                            {
                                await Task.Delay(DelayGenerator(DelayAfterKillPoint));
                            }
                        }
                        i++;
                    }
                    
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
            }
        }
        private async Task SelfDefence()
        {
            map_changed = false;
            if (Scene.LastAttacks.Count > 0 && run && !map_changed)
            {
                
                AddLog("Killing the monsters that attacked to us", "Self Defence");
                while (Scene.LastAttacks.Count > 0 && run && !map_changed)
                {
                    Scene.Entities entity = Scene.LastAttacks.Values.ElementAt(0);
                    if (!Scene.EntityData.ContainsKey(entity.Id) || (DateTime.Now - entity.LastAttack).TotalSeconds > 10.0)
                        Scene.LastAttacks.Remove(entity.Id);
                    else
                    {
                        NeedHelp = true;
                        while (Scene.EntityData.ContainsKey(entity.Id) && run && !map_changed)
                        {
                            HelpNeededMobId = entity.Id;
                            Api.attack_monster(entity.Id);
                            await Task.Delay(200);
                        }
                    }
                    await Task.Delay(10);
                }
                await Task.Delay(1000);
            }
            await Loot();
            NeedHelp = false;
            HelpNeededMobId = -1;
            if (!run)
            {
                Stop();
                return;
            }
        }
        private async Task Amulet()
        {
            if (!run)
            {
                Stop();
                return;
            }
            Player.updated = false;
            Api.query_inventory();
            while (!Player.updated)
                await Task.Delay(1);
            var item = Player.Inventory.Where(x => x.Vnum == 2071).FirstOrDefault();
            if (item == null)
            {
                AddLog($"Couldn't find item by vnum {2071}", "Amulet");
                return;
            }
            map_changed = false;
            await Task.Delay(DelayGenerator(AmuletUseDelay));
            while (!map_changed && run)
            {
                AddLog("Trying to use amulet", "Amulet");
                Api.send_packet($"u_i 1 {Player.id} 2 {item.Pos} 0 0");
                await Task.Delay(1000);
                Api.send_packet($"#u_i^1^{Player.id}^2^{item.Pos}^1");
                for (int i = 0; i < 90; i++)
                {
                    if (HelpAmulet || map_changed || Scene.LastAttacks.Count > 0 || !run || i > 10 && !Player.Dancing)
                        break;
                    await Task.Delay(100);
                }
                if (!run)
                {
                    Stop();
                    continue;
                }
                else if (map_changed)
                {
                    continue;
                }
                else if (HelpAmulet)
                {
                    await GoHelp();
                }
                else if (Scene.LastAttacks.Count > 0)
                {
                    await SelfDefence();
                }
                await Task.Delay(random.Next(1000, 1450));
            }
            

        }
        private async Task GoHelp()
        {
            while (HelpAmulet && DefenceMob > 0 && !map_changed && run)
            {
                Api.attack_monster(DefenceMob);
                await Task.Delay(200);
            }
            if (!run)
            {
                Stop();
                return;
            }
        }
        private async Task Loot()
        {
            if (!run)
            {
                Stop();
                return;
            }
            await Task.Delay(random.Next(1000, 1750));
            AddLog("Started looting", "Loot");
            
            bool ignoreflower = true;
            if (!IgnoreFlower)
            {
                ignoreflower = false;
            }
            else if (Player.FlowerSW.Elapsed.TotalSeconds == 0 && !Player.StrongFlower)
            {
                ignoreflower = false;
            }
            else if (Player.FlowerSW.Elapsed.Minutes > 7 && Player.NormalFlower)
            {
                ignoreflower = false;
            }
            else if (!Player.NormalFlower && !Player.StrongFlower)
            {
                ignoreflower = false;
            }
            var loot = Scene.GetLoot(LootSearchRadius, LootBlacklist, LootWhiteList, ignoreflower, MyItems, GroupItems, NeutralItems, LootList);
            Stopwatch sw = Stopwatch.StartNew();
            map_changed = false;
            while (loot.Id != -1 && run && !map_changed)
            {
                await Task.Delay(10);
                ignoreFlower = ShouldIgnoreFlower();
                if (Statics.Distance(loot.Pos, new Point(Multi_Account_Synchronizer.Player.x, Multi_Account_Synchronizer.Player.y)) > Math.Sqrt(2))
                {
                    Api.player_walk(loot.Pos.X, loot.Pos.Y);
                }
                else
                {
                    var list = Multi_Account_Synchronizer.Scene.GetLootList(LootBlacklist, LootWhiteList, ignoreFlower, MyItems, GroupItems, NeutralItems, LootList, TrashItems);
                    foreach (var lood in list)
                    {
                        AddLog($"Looting item with id {lood.Id} and vnum {lood.Vnum}", "Loot");
                        while (Multi_Account_Synchronizer.Scene.LootData.ContainsKey(lood.Id) && run)
                        {
                            CurrentLootId = lood.Id;
                            Api.pick_up(lood.Id);
                            await Task.Delay(200);
                            if (sw.Elapsed.TotalSeconds >= IgnoreTıme && IgnoreItem && Multi_Account_Synchronizer.Scene.LootData.ContainsKey(loot.Id))
                            {
                                Multi_Account_Synchronizer.Scene.LootData.Remove(loot.Id);
                                sw.Restart();
                            }
                            sw.Restart();
                        }
                    }
                }

                
                loot = Multi_Account_Synchronizer.Scene.GetLoot(LootSearchRadius, LootBlacklist, LootWhiteList, ignoreFlower, MyItems, GroupItems, NeutralItems, LootList);
                sw.Restart();
                await Task.Delay(10);
            }
            CurrentLootId = -1;
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
            await Task.Delay(DelayGenerator(StartAttackDelay));
            Stopwatch luresw = Stopwatch.StartNew();
            Stopwatch targetsw = Stopwatch.StartNew();
            int oldmob = LureMob;
            while (LureMob > 0 && !map_changed && run)
            {
                Api.attack_monster(LureMob);
                await Task.Delay(200);
                if (oldmob != LureMob)
                {
                    oldmob = LureMob;
                    targetsw.Restart();
                }
                if (oldmob == LureMob && IgnoreTarget && targetsw.Elapsed.TotalSeconds >= IgnoreTargetValue)
                {
                    if (Scene.EntityData.ContainsKey(LureMob))
                        Scene.EntityData.Remove(LureMob);
                }
                if (UseVoke && luresw.Elapsed.TotalSeconds >= 3)
                {
                    if (Panda && IsVokeReady() && VokeMonsterCount() >= MinVokeMonsterCount)
                    {
                        Api.use_pet_skill(LureMob, 1714);
                    }
                    if (Otter)
                    {
                        var mob = Scene.EntityData.Values.Where(x => x.Id == LureMob).FirstOrDefault();
                        if (mob != null)
                        {
                            
                            
                            if (Statics.Distance(mob.Pos, new Point(Player.Pet.X,Player.Pet.Y)) <= Math.Sqrt(2) && VokeMonsterCount() >= MinVokeMonsterCount)
                            {
                                Api.use_pet_skill(LureMob, 663);
                            }
                            else
                            {
                                Api.pets_walk(mob.Pos.X, mob.Pos.Y);
                            }
                            
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
        private int VokeMonsterCount()
        {
            int count = 0;
            if (Otter)
            {
                count = Scene.MonstersInRadius(Player.Pet.X, Player.Pet.Y, 6, AttackBlacklist, AttackWhitelist, MonsterList);
            }
            else if (Panda && Scene.EntityData.ContainsKey(LureMob))
            {
                count = Scene.MonstersInRadius(Scene.EntityData[LureMob].Pos.X, Scene.EntityData[LureMob].Pos.Y,6, AttackBlacklist, AttackWhitelist, MonsterList);
            }
            return count;
        }
        private int VokeMonsterCount()
        {
            int count = 0;
            if (Otter)
            {
                count = Multi_Account_Synchronizer.Scene.MonstersInRadius(Multi_Account_Synchronizer.Player.Pet.X, Multi_Account_Synchronizer.Player.Pet.Y, 6, AttackBlacklist, AttackWhitelist, MonsterList);
            }
            else if (Panda && Multi_Account_Synchronizer.Scene.EntityData.ContainsKey(LureMob))
            {
                count = Multi_Account_Synchronizer.Scene.MonstersInRadius(Multi_Account_Synchronizer.Scene.EntityData[LureMob].Pos.X, Multi_Account_Synchronizer.Scene.EntityData[LureMob].Pos.Y,6, AttackBlacklist, AttackWhitelist, MonsterList);
            }
            return count;
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
            while (Player.map_id != 20001 && !map_changed && run)
                await Task.Delay(100);
            if (!run)
            {
                WaitingForMiniland = false;
                ResetMinilandsw = true;
                Stop();
                return;
            }
            if (Player.map_id != 20001)
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
                Api.send_packet($"$Invite {account}");
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
                Api.send_packet($"u_s {id.Item2} 1 {Player.id}");
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
            await Task.Delay(DelayGenerator(MinilandExitDelay));
            while (Player.map_id == 20001)
            {
                Api.player_walk(x, y);
                Api.pets_walk(x, y);
                await Task.Delay(1500);
                if (sw.Elapsed.TotalSeconds >= 5)
                {
                    x = 3 + random.Next(-1, 2);
                    y = 8 + random.Next(-1, 2);
                    sw.Restart();
                }
            }
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
            Stopwatch sw = Stopwatch.StartNew();
            while (!File.Exists(newpath) && sw.Elapsed.TotalSeconds < 5)
                await Task.Delay(500);
            if (!File.Exists(newpath))
                return;
            Api.stop_bot();
            Api.load_settings(newpath);
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
            else if (header == "u_s")
                handle_us(packet_splitted, full_packet);
            else if (header == "su")
                handle_su(packet_splitted, full_packet);
            else if (header == "sayi")
                handle_sayi(packet_splitted, full_packet);

        }

        private void handle_sayi(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count < 5)
                return;
            //this item belongs to different party
            if (packet_splitted[1] == "1" && packet_splitted[2] == Player.id.ToString() && packet_splitted[4] == "544")
            {
                if (CurrentLootId == -1)
                    return;
                if (Scene.LootData.ContainsKey(CurrentLootId))
                    Scene.LootData.Remove(CurrentLootId);
            }

        }

        private void handle_su(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted[1] == "1" && packet_splitted[2] == Player.id.ToString() && packet_splitted[3] == "3")
            {
                if (Player.map_id == 20001 || KeepDistanceValue == 0 || !KeepDistance || !run)
                    return;
                int x;
                int y;
                int mobid = LastAttackedMob;
                var entity = Scene.EntityData.Values.Where(ent => ent.Id == mobid).FirstOrDefault();
                if (entity == null)
                {
                    return;
                }
                else
                {
                    int atanx = Player.x - entity.Pos.X;
                    int atany = Player.y - entity.Pos.Y;
                    double radian = Math.Atan2(atany, atanx);
                    int cosx = (int)Math.Round(Math.Cos(radian));
                    int sinx = (int)Math.Round(Math.Sin(radian));
                    x = Player.x + cosx;
                    y = Player.y + sinx;
                    if (Statics.Distance(new Point(Player.x, Player.y), entity.Pos) >= KeepDistanceValue)
                        return;

                    Api.player_walk(x, y);
                }

            }
        }

        private void handle_us(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted[2] != "3")
                return;
            LastAttackedMob = int.Parse(packet_splitted[3]);
            var mob = Scene.EntityData.Values.Where(x => x.Id == LastAttackedMob).FirstOrDefault();
            if (mob != null && KeepDistance && KeepDistanceValue != 0)
            {
                Stopwatch sw = Stopwatch.StartNew();
                while (!Attacked && run)
                {
                    await Task.Delay(100);
                    if (sw.Elapsed.TotalSeconds >= 5)
                        return;
                }
                if (!run)
                    return;
                WalkRangePoint(mob.Pos, KeepDistanceValue);

            }
        }

        private async void handle_qnamli2(List<string> packet_splitted, string full_packet)
        {
            //miniland invite
            if (full_packet.Contains("#mjoin") && full_packet.Contains(OwnerName))
            {
                await Task.Delay(DelayGenerator(AcceptInviteDelay));
                Api.send_packet(packet_splitted[2]);
            }
                
        }

        private void handle_c_map(List<string> packet_splitted, string full_packet)
        {
            //c_map 0 2640 1
            //Map change security
            if (packet_splitted.Count < 4)
                return;
            if (packet_splitted[3] != "1")
                return;
            if (!SecurityEnabled)
                return;
            if (!MapStop)
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
            //Loading map cells, 1 = walkable, 0 = unwalkable
            if (packet_splitted[2] != LastLoadedMap.ToString() && packet_splitted[2] != "20001")
            {
                CurrentMap = Statics.LoadMap(int.Parse(packet_splitted[2]));
                LastLoadedMap = int.Parse(packet_splitted[2]);
            }
        }
    }
}
