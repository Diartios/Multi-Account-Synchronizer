using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace Multi_Account_Synchronizer
{
    internal class PhoenixApi
    {
        TcpClient client = new TcpClient();
        NetworkStream clientz;
        List<string> message = new List<string>();
        static System.Timers.Timer timer = new System.Timers.Timer(10);

        byte[] buffer = new byte[4096];
        string data;
        int delim_pos;
        public bool run = true;
        public enum Type
        {
            packet_send,
            packet_recv,
            attack,
            player_skill,
            player_walk,
            pet_skill,
            partner_skill,
            pets_walk,
            pick_up,
            collect,
            start_bot,
            stop_bot,
            continue_bot,
            load_settings,
            start_minigame_bot,
            stop_minigame_bot,
            query_player_info,
            query_inventory,
            query_skills_info,
            query_map_entities,
            target
        }


        public bool ConnectPort(int port)
        {
            bool zort = false;
            try
            {
                client.Connect("127.0.0.1", port);
                clientz = client.GetStream();
                zort = true;

            }
            catch (Exception)
            {

                zort = false;
            }

            return zort;
        }
        public void Close()
        {
            client.Close();
            clientz.Close();
        }
        public void send_data(string data)
        {
            try
            {
                string msg = data + '\x01';
                var sendbytes = System.Text.Encoding.UTF8.GetBytes(msg);
                clientz.Write(sendbytes, 0, sendbytes.Length);
            }
            catch (Exception)
            {
            }
        }

        public void send_packet(string packet)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.packet_send);
            paket["packet"] = packet;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void receive_packet(string packet)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.packet_recv);
            paket["packet"] = packet;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void attack_monster(int monster_id, int entity_type = 3)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.attack);
            paket["monster_id"] = monster_id;
            string zort = paket.ToString();
            target_entity(monster_id, entity_type);
            send_data(zort);
        }
        public void target_entity(int entity_id, int entity_type = 3)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.target);
            paket["entity_type"] = entity_type;
            paket["entity_id"] = entity_id;
            string zort = paket.ToString();
            send_data(zort);
        }
        public void use_player_skill(int monster_id, int skill_id, int entity_type = 3)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.player_skill);
            paket["monster_id"] = monster_id;
            paket["skill_id"] = skill_id;
            target_entity(monster_id, entity_type);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void player_walk(int x, int y)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.player_walk);
            paket["x"] = x;
            paket["y"] = y;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void use_pet_skill(int monster_id, int skill_id)
        {
            JObject paket = new JObject();
            paket["type"] = (int)Type.pet_skill;
            paket["monster_id"] = monster_id;
            paket["skill_id"] = skill_id;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void use_partner_skill(int monster_id, int skill_id)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.partner_skill);
            paket["monster_id"] = monster_id;
            paket["skill_id"] = skill_id;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void pets_walk(int x, int y)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.pets_walk);
            paket["x"] = x;
            paket["y"] = y;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void pick_up(int item_id)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.pick_up);
            paket["item_id"] = item_id;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void collect(int npc_id)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.collect);
            paket["npc_id"] = npc_id;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void start_bot()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.start_bot);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void stop_bot()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.stop_bot);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void continue_bot()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.continue_bot);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void load_settings(string settings_path)
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.load_settings);
            paket["path"] = settings_path;
            string zort = paket.ToString();
            send_data(zort);
        }

        public void start_minigame_bot()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.start_minigame_bot);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void stop_minigame_bot()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.stop_minigame_bot);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void query_player_info()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.query_player_info);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void query_inventory()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.query_inventory);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void query_skills_info()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.query_skills_info);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void query_map_entities()
        {
            JObject paket = new JObject();
            paket["type"] = ((int)Type.query_map_entities);
            string zort = paket.ToString();
            send_data(zort);
        }

        public void receive_message()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            zurt();
        }

        public string get_message()
        {
            if (message.Count == 0) return "";

            string pirt = message[0];
            message.Remove(pirt);
            if (pirt == null)
                return "";
            return pirt;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        public async void zurt()
        {
            while (run)
            {
                if (clientz.DataAvailable && run)
                {
                    int bytesReceived = clientz.Read(buffer, 0, buffer.Length);
                    data += Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    delim_pos = data.IndexOf('\u0001');
                    while (delim_pos != -1)
                    {
                        if (data.IndexOf('\u0001') != -1)
                        {
                            string zart = data.Substring(0, delim_pos - 1);
                            data = data.Remove(0, delim_pos + 1);
                            message.Add(zart);
                        }
                        delim_pos = data.IndexOf('\u0001');
                    }
                }
                else
                    await Task.Delay(1);
            }
        }

        public PhoenixApi()
        {

        }
    }
}
