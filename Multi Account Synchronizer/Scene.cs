using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    internal class Scene
    {
        public Player player = new Player();
        public Dictionary<int, Entities> EntityData = new Dictionary<int, Entities>();
        public Dictionary<int, Loot> LootData = new Dictionary<int, Loot>();
        public Dictionary<int, Scene.Entities> LastAttacks = new Dictionary<int, Scene.Entities>();
        public bool updated = false;
        public int MobID()
        {
            double maxdistance = 1000;
            int mob_id = -1;
            try
            {
                foreach (Entities entity in EntityData.Values)
                {
                    double distance = Math.Sqrt(Math.Pow(player.x - entity.Pos.X, 2) + Math.Pow(player.y - entity.Pos.Y, 2));
                    if (distance < maxdistance)
                    {
                        maxdistance = distance;
                        mob_id = entity.Id;
                    }
                }
            }
            catch (Exception)
            {

            }

            return mob_id;
        }
        public int CenterMob(int radius, bool blacklist, bool whitelist, List<int> monsters, bool priority)
        {
            int mobid = -1;
            double maxdistance = 1000;
            if (blacklist)
                priority = false;
            try
            {
                List<Entities> ids = new List<Entities>();
                foreach (Entities entity in EntityData.Values)
                {
                    if (blacklist && monsters.Contains(entity.Vnum))
                        continue;
                    if (whitelist && !monsters.Contains(entity.Vnum))
                        continue;
                    if (Statics.Distance(new Point(player.x, player.y), entity.Pos) > radius)
                        continue;
                    ids.Add(entity);
                }
                if (ids.Count <= 0)
                    return -1;
                int x = ids.Sum(t => t.Pos.X);
                int y = ids.Sum(t => t.Pos.Y);
                double totalx = Convert.ToDouble(x) / Convert.ToDouble(ids.Count);
                double totaly = Convert.ToDouble(y) / Convert.ToDouble(ids.Count);
                mobid = ids[0].Id;
                int vnum = -1;
                foreach (Entities entity in ids)
                {
                    int indexnew = monsters.IndexOf(entity.Vnum);
                    int indexold = monsters.IndexOf(vnum);
                    
                    
                    double distance = Math.Sqrt(Math.Pow(totalx - entity.Pos.X, 2) + Math.Pow(totaly - entity.Pos.Y, 2));
                    if ((indexnew < indexold || indexold == -1) && priority)
                    {
                        maxdistance = distance;
                        mobid = entity.Id;
                        vnum = entity.Vnum;
                    }
                    else if (indexnew == indexold && priority && distance < maxdistance)
                    {
                        maxdistance = distance;
                        mobid = entity.Id;
                    }
                    else if (distance < maxdistance && !priority)
                    {
                        maxdistance = distance;
                        mobid = entity.Id;
                    }
                }
                return mobid;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public Loot GetLoot(int radius, bool blacklist, bool whitelist, bool ignoreflowers, bool my, bool group, bool neutral, List<int> lootlist)
        {
            Loot resultloot = new Loot(-1, -1, new Point(-1, -1), 0, -1, DateTime.Now);
            double maxdistance = 1000;
            List<int> owners = new List<int>();
            if (my)
                owners.Add(player.id);
            if (group)
                owners.Add(-1);
            if (neutral)
                owners.Add(0);
            foreach (Loot loot in LootData.Values)
            {
                if (Statics.Distance(new Point(player.x, player.y), loot.Pos) > radius)
                    continue;
                if (blacklist && lootlist.Contains(loot.Vnum))
                    continue;
                if (whitelist && !lootlist.Contains(loot.Vnum))
                    continue;
                if (loot.Vnum == 1086 && ignoreflowers)
                    continue;
                if (!owners.Contains(loot.Owner))
                    continue;
                double distance = Statics.Distance(new Point(player.x,player.y), loot.Pos);
                if (distance < maxdistance)
                {
                    maxdistance = distance;
                    resultloot = loot;
                }

            }
            return resultloot;
        }
        public List<Loot> GetLootList(bool blacklist, bool whitelist, bool ignoreflowers, bool my, bool group, bool neutral, List<int> lootlist, bool trashitems)
        {
            Random rnd = new Random();
            List<Loot> resultList = new List<Loot>();
            List<int> owners = new List<int>();
            if (my)
                owners.Add(player.id);
            if (group)
                owners.Add(-1);
            if (neutral)
                owners.Add(0);
            foreach (Loot loot in LootData.Values)
            {
                int addchance = rnd.Next(0, 4);
                if (!trashitems)
                    addchance = 69;
                if (Statics.Distance(new Point(player.x, player.y), loot.Pos) > Math.Sqrt(2))
                    continue;
                if (blacklist && lootlist.Contains(loot.Vnum) && addchance != 0)
                    continue;
                if (whitelist && !lootlist.Contains(loot.Vnum) && addchance != 0)
                    continue;
                if (loot.Vnum == 1086 && ignoreflowers)
                    continue;
                if (!owners.Contains(loot.Owner))
                    continue;
                resultList.Add(loot);
            }
            return resultList;
        }
        public void handle_packets(List<string> packet_splitted, string full_packet)
        {
            string header = packet_splitted[0];

            if (header == "su")
                handle_su(packet_splitted, full_packet);
            else if (header == "mv")
                handle_mv(packet_splitted, full_packet);
            else if (header == "at")
                handle_at(packet_splitted, full_packet);
            else if (header == "out")
                handle_out(packet_splitted, full_packet);
            else if (header == "in")
                handle_in(packet_splitted, full_packet);
            else if (header == "get")
                handle_get(packet_splitted, full_packet);
            else if (header == "die")
                handle_die(packet_splitted, full_packet);
            else if (header == "drop")
                handle_drop(packet_splitted, full_packet);
        }
        public void handle_drop(List<string> packet_splitted, string full_packet)
        {
            int vnum = int.Parse(packet_splitted[1]);
            int id = int.Parse(packet_splitted[2]);
            int x = int.Parse(packet_splitted[3]);
            int y = int.Parse(packet_splitted[4]);
            int quantity = int.Parse(packet_splitted[5]);
            int owner = int.Parse(packet_splitted[7]);
            Point position = new Point(x, y);
            Loot loot = new Loot(vnum, id, position, quantity, owner, DateTime.Now);
            LootData[id] = loot;
            
        }
        public void handle_die(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 5)
                return;
            int id1;
            int id2;
            int type1;
            int type2;
            try
            {
                id1 = int.Parse(packet_splitted[2]);
                id2 = int.Parse(packet_splitted[4]);
                type1 = int.Parse(packet_splitted[1]);
                type2 = int.Parse(packet_splitted[3]);
                if (type1 == type2 && id1 == id2)
                {
                    EntityData.Remove(id1);
                }
                   
            }
            catch (Exception)
            {

            }
        }

        public void handle_get(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 5) return;
            try
            {
                if (LootData.ContainsKey(int.Parse(packet_splitted[3])) && packet_splitted[4] == "0")
                {
                    LootData.Remove(int.Parse(packet_splitted[3]));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(full_packet + " " + e.Message);
            }
        }

        public void handle_su(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 13) return;

            int id;
            int type;
            int isdead;

            try
            {
                id = int.Parse(packet_splitted[4]);
                type = int.Parse(packet_splitted[3]);
                isdead = int.Parse(packet_splitted[11]);

                if (type == 3 && isdead == 0 && EntityData.ContainsKey(id))
                {
                    if (EntityData[id].Vnum == 1500)
                        Console.WriteLine($"removed mob with vnum 1500 {full_packet}");
                    EntityData.Remove(id);
                }
                if (type == 3 && isdead == 0)
                {
                    if (EntityData.ContainsKey(id))
                        EntityData.Remove(id);
                }
                if (packet_splitted[1] == "3" && type == 1 && id == player.id)
                {
                    int attackerid = int.Parse(packet_splitted[2]);
                    Entities entities = new Entities { Id = attackerid, LastAttack = DateTime.Now };
                    LastAttacks[attackerid] = entities; 
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show(full_packet, "ERROR");
            }
        }

        public void handle_mv(List<string> packet_splitted, string full_packet)
        {
            // mv 3 3425 68 136 6
            if (packet_splitted.Count() < 5)
                return;
            int id = -1;
            int type = -1;
            int x = -1;
            int y = -1;

            try
            {
                id = int.Parse(packet_splitted[2]);
                type = int.Parse(packet_splitted[1]);
                x = int.Parse(packet_splitted[3]);
                y = int.Parse(packet_splitted[4]);
            }
            catch (Exception)
            {

            }

            if (type == 3 && EntityData.ContainsKey(id))
            {
                EntityData[id].Pos = new Point(x, y);
            }
            else if (!EntityData.ContainsKey(id) && type == 3)
                EntityData.Add(id, new Entities { Id = id, Pos = new Point(x, y) });
        }

        public void handle_at(List<string> packet_splitted, string full_packet)
        {
            EntityData.Clear();
            LootData.Clear();
        }

        public void handle_out(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 3) return;

            int id;
            int type;
            try
            {
                type = int.Parse(packet_splitted[1]);
                id = int.Parse(packet_splitted[2]);
                if (LootData.ContainsKey(id) && type == 9)
                {
                    LootData.Remove(id);
                }
            }
            catch (Exception)
            {

                MessageBox.Show(full_packet, "ERROR");
            }

        }

        public void handle_in(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 5) return;
            if (packet_splitted[1] == "1") return;
            //in 9 1000 5444154 14 15 1 0 0 0
            int x = -1;
            int y = -1;
            int id = -1;
            int vnum = -1;
            int type = -1;
            try
            {
                type = int.Parse(packet_splitted[1]);
                vnum = int.Parse(packet_splitted[2]);
                id = int.Parse(packet_splitted[3]);
                x = int.Parse(packet_splitted[4]);
                y = int.Parse(packet_splitted[5]);
            }
            catch (Exception)
            {
                MessageBox.Show(full_packet, "ERROR");
            }
            if (id == -1 || vnum == -1 || x == -1 || y == -1 || type == -1) { return; }
            if (type == 3)
            {
                if (vnum == 2104)
                    return;
                EntityData[id] = new Entities { Id = id, Pos = new Point(x, y), Vnum = vnum };
            }
            if (type == 9)
            {
                int quantity = int.Parse(packet_splitted[6]);
                int owner = int.Parse(packet_splitted[9]);
                Point position = new Point(x, y);
                Loot loot = new Loot(vnum, id, position, quantity, owner, DateTime.Now);
                LootData[id] = loot;
            }
        }

        public class Entities
        {
            public int Id { get; set; }
            public Point Pos { get; set; }
            public int Vnum { get; set; }
            public DateTime LastAttack { get; set; }
        }

        public class Drops
        {
            public int Id { get; set; }
            public Point Pos { get; set; }
        }
    }
}
