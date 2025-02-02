using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    internal class Player : IDisposable
    {
        public int x = -1;
        public int y = -1;
        public string name = "";
        public string all = "";
        public int id = -1;
        public int map_id = -1;
        public double hp = 100;
        public double mp = 100;
        public PhoenixApi phoenixapi = new PhoenixApi();
        public List<Item> Inventory = new List<Item>();
        public List<Tuple<string, int>> Skills = new List<Tuple<string, int>>();
        public Dictionary<int, bool> SkillReady = new Dictionary<int, bool>();
        public bool updated = false;
        public bool NormalFlower = false;
        public bool StrongFlower = false;
        public bool FlowerQuest = false;
        public bool Dancing = false;
        public Stopwatch FlowerSW = new Stopwatch();
        public Pet Pet = new Pet();
        public string PartnerId = "";
        public class Item
        {
            public string Name { get; set; }
            public int Vnum { get; set; }
            public int Quantity { get; set; }
            public int Pos { get; set; }
            public int Type { get; set; }
        }
       
        public void handle_skills(string info)
        {
            Skills.Clear();
            SkillReady.Clear();
            JObject skills = JObject.Parse(info);
            foreach (var skill in skills["skills"])
            {
                string name = skill["name"].ToString();
                int id = ((int)skill["id"]);
                bool isReady = ((bool)skill["is_ready"]);
                Tuple<string, int> obj = new Tuple<string, int>(name, id);
                SkillReady[id] = isReady;
                Skills.Add(obj);
            }
            updated = true;
        }
        public void handle_inventory(string inv)
        {
            JObject inventory = JObject.Parse(inv);
            Inventory.Clear();
            foreach (var item in inventory["inventory"]["equip"])
            {
                Item i = new Item { Name = item["name"].ToString(), Pos = ((int)item["position"]), Quantity = ((int)item["quantity"]), Vnum = ((int)item["vnum"]), Type = 0 };
                Inventory.Add(i);
            }
            foreach (var item in inventory["inventory"]["main"])
            {
                Item i = new Item { Name = item["name"].ToString(), Pos = ((int)item["position"]), Quantity = ((int)item["quantity"]), Vnum = ((int)item["vnum"]), Type = 1 };
                Inventory.Add(i);
            }
            foreach (var item in inventory["inventory"]["etc"])
            {
                Item i = new Item { Name = item["name"].ToString(), Pos = ((int)item["position"]), Quantity = ((int)item["quantity"]), Vnum = ((int)item["vnum"]), Type = 2 };
                Inventory.Add(i);
            }
            updated = true;
        }
        public void handle_packets(List<string> packet_splitted, string full_packet)
        {
            string header = packet_splitted[0];
            if (header == "walk")
                handle_walk(packet_splitted, full_packet);
            else if (header == "at")
                handle_at(packet_splitted, full_packet);
            else if (header == "guri")
                handle_guri(packet_splitted, full_packet);
            else if (header == "tp")
                handle_tp(packet_splitted, full_packet);
            else if (header == "stat")
                handle_stat(packet_splitted, full_packet);
            else if (header == "cond")
                handle_cond(packet_splitted, full_packet);
            else if (header == "c_info")
                handle_cinfo(packet_splitted, full_packet);
            else if (header == "bf")
                handle_bf(packet_splitted, full_packet);
            else if (header == "petski")
                handle_petski(packet_splitted, full_packet);
            else if (header == "pet_cool2")
                handle_pet_cool2(packet_splitted, full_packet);
            else if (header == "petsr")
                handle_petsr(packet_splitted, full_packet);
            else if (header == "ptctl")
                handle_ptctl(packet_splitted, full_packet);
            else if (header == "in")
                handle_in(packet_splitted, full_packet);
            else if (header == "pst")
                handle_pst(packet_splitted, full_packet);
            else if (header == "qstlist")
                handle_qstlist(packet_splitted, full_packet);
        }
        private void handle_qstlist(List<string> packet_splitted, string full_packet)
        {
            FlowerQuest = false;
            for (int i = 1; i < packet_splitted.Count; i++)
            {
                string[] newSplit = packet_splitted[i].Split('.');
                if (newSplit.Count() < 2)
                    continue;
                if (newSplit[1] == "5981")
                    FlowerQuest = true;
            }
            Console.WriteLine($"{name}, {FlowerQuest}");
        }
        public void handle_pst(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted[1] == "2" && packet_splitted[3] == "1")
            {
                Pet.Id = int.Parse(packet_splitted[2]);
            }
            if (packet_splitted[1] == "2" && packet_splitted[3] == "0")
            {
                PartnerId = packet_splitted[2];
            }
        }
        public void handle_in(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted[1] == "1")
                return;

            int vnum = int.Parse(packet_splitted[2]);
            int x = int.Parse(packet_splitted[4]);
            int y = int.Parse(packet_splitted[5]);
            if (packet_splitted[1] == "2" && packet_splitted[3] == Pet.Id.ToString())
            {
                Pet.X = x;
                Pet.Y = y;
            }
        }
        public void handle_ptctl(List<string> packet_splitted, string full_packet)
        {
            map_id = int.Parse(packet_splitted[1]);
            if (packet_splitted[2] == "2")
            {
                int x1 = int.Parse(packet_splitted[4]);
                int y1 = int.Parse(packet_splitted[5]);
                int x2 = int.Parse((packet_splitted[7]));
                int y2 = int.Parse(packet_splitted[8]);
                if (packet_splitted[3] == Pet.Id.ToString())
                {
                    Pet.X = x1;
                    Pet.Y = y1;
                }
                else if (packet_splitted[3] == PartnerId)
                {
                    Pet.X = x2;
                    Pet.Y = y2;
                }
            }
            else if (packet_splitted[2] == "1")
            {
                int x = int.Parse(packet_splitted[4]);
                int y = int.Parse(packet_splitted[5]);
                if (packet_splitted[3] == Pet.Id.ToString())
                {
                    Pet.X = x;
                    Pet.Y = y;
                }
            }
        }
        public void handle_petski(List<string> packet_splitted, string full_packet)
        {
            Pet.Skills.Clear();
            for (int i = 1; i < packet_splitted.Count; i++)
            {
                int skillvnum = int.Parse(packet_splitted[i]);
                if (skillvnum == -1)
                    continue;
                Pet.Skills[skillvnum] = false;
            }
        }
        public void handle_pet_cool2(List<string> packet_splitted, string full_packet)
        {
            for (int i = 2; i < packet_splitted.Count; i++)
            {
                if (i > Pet.Skills.Count + 1)
                    break;
                int key = Pet.Skills.ElementAt(i - 2).Key;
                Pet.Skills[key] = packet_splitted[i] == "0";
            }
        }
        public void handle_petsr(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count < 2)
                return;
            int index = int.Parse(packet_splitted[1]);
            if (index >= Pet.Skills.Count)
                return;
            int key = Pet.Skills.ElementAt(index).Key;
            Pet.Skills[key] = true;
        }
        public void handle_bf(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count < 5)
                return;
            if (packet_splitted[1] != "1")
                return;
            if (packet_splitted[2] != id.ToString())
                return;
            string[] split = packet_splitted[3].Split('.');
            if (split[1] == "378" && split[2] != "0")
            {
                NormalFlower = true;
                FlowerSW.Restart();
            }
            else if (split[1] == "378" && split[2] == "0")
            {
                NormalFlower = false;
                FlowerSW.Reset();
            }
            else if (split[1] == "379" && split[2] != "0")
            {
                StrongFlower = true;
                FlowerSW.Restart();
            }
            else if (split[1] == "379" && split[2] == "0")
            {
                StrongFlower = false;
                FlowerSW.Reset();
            }

        }
        public void handle_cinfo(List<string> packet_splitted, string full_packet)
        {
            try
            {
                name = packet_splitted[1];
            }
            catch (Exception e)
            {
                MessageBox.Show(full_packet + " " + e.Message);
            }
        }
        public void handle_stat(List<string> packet_splitted, string full_packet)
        {
            //stat 1096 3740 4577 4927 0 1056
            try
            {
                double currenthp = int.Parse(packet_splitted[1]);
                double maxhp = int.Parse(packet_splitted[2]);
                double currentmp = int.Parse(packet_splitted[3]);
                double maxmp = int.Parse(packet_splitted[4]);
                hp = (currenthp / maxhp) * 100;
                mp = (currentmp / maxmp) * 100;
            }
            catch (Exception e)
            {
                MessageBox.Show(full_packet + " " + e.Message);
            }
        }

        public void handle_player_infos(string playe_info)
        {
            JObject player_info = JObject.Parse(playe_info);
            x = ((int)player_info["player_info"]["x"]);
            y = ((int)player_info["player_info"]["y"]);
            name = player_info["player_info"]["name"].ToString();
            id = ((int)player_info["player_info"]["id"]);
            map_id = ((int)player_info["player_info"]["map_id"]);
            all = playe_info;
        }
        public void handle_cond(List<string> packet_splitted, string full_packet)
        {
            //cond 1 6830366 0 0 16
            try
            {
                if (packet_splitted[1] == "1")
                    id = int.Parse(packet_splitted[2]);
            }
            catch (Exception e)
            {
                MessageBox.Show(full_packet + " " + e.Message);
            }
        }
        public void handle_walk(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 3) return;
            Dancing = false;
            try
            {
                x = int.Parse(packet_splitted[1]);
                y = int.Parse(packet_splitted[2]);
            }
            catch (Exception e)
            {
                MessageBox.Show(full_packet + " " + e.Message);
            }
        }

        public void handle_at(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 5) return;
            Dancing = false;
            try
            {
                id = int.Parse(packet_splitted[1]);
                map_id = int.Parse(packet_splitted[2]);
                x = int.Parse(packet_splitted[3]);
                y = int.Parse(packet_splitted[4]);
            }
            catch (Exception)
            {
                MessageBox.Show(full_packet, "ERROR");
            }
        }

        public void handle_guri(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 5)
                return;
            if (packet_splitted[1] == "5" && packet_splitted[2] == "1" && packet_splitted[3] == id.ToString() && packet_splitted[4] == "0")
                Dancing = true;
            else if (packet_splitted[1] == "5" && packet_splitted[2] == "1" && packet_splitted[3] == id.ToString() && packet_splitted[4] == "-1")
                Dancing = false;
            if (packet_splitted.Count() < 9)
                return;
            try
            {
                if (packet_splitted[1] == "3" && packet_splitted[2] == "1" && packet_splitted[3] == id.ToString())
                {
                    x = int.Parse(packet_splitted[4]);
                    y = int.Parse(packet_splitted[5]);
                }
                if (packet_splitted[1] == "3" && packet_splitted[2] == "2" && packet_splitted[3] == Pet.Id.ToString())
                {
                    Pet.X = int.Parse(packet_splitted[4]);
                    Pet.Y = int.Parse(packet_splitted[5]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("handle_guri " + full_packet + " " + e.Message);
            }
        }

        public void handle_tp(List<string> packet_splitted, string full_packet)
        {
            if (packet_splitted.Count() < 4)
                return;
            if (packet_splitted[1] == "1" && packet_splitted[2] == id.ToString())
            {
                x = int.Parse(packet_splitted[3]);
                y = int.Parse(packet_splitted[4]);
            }
            if (packet_splitted[1] == "2" && packet_splitted[2] == Pet.Id.ToString())
            {
                Pet.X = int.Parse(packet_splitted[3]);
                Pet.Y = int.Parse(packet_splitted[4]);
            }
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios:
        // If disposing equals true, the method has been called directly or indirectly by a user's code, so managed and unmanaged resources can be disposed.
        // If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Dispose managed resources here.
            }

            // Dispose unmanaged resources here.
            disposed = true;
        }

        // The finalizer.
        ~Player()
        {
            Dispose(false);
        }
    }
}
