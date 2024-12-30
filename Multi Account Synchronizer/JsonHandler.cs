using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    internal class JsonHandler
    {
        static System.Timers.Timer timer = new System.Timers.Timer(10); // 10ms interval
        public PhoenixApi phoenixapi = new PhoenixApi();
        public Player player = new Player();
        public Scene scene = new Scene();
        public Bot bot = new Bot();
        public bool run = true;
        public void start()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Handler();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        private async void Handler()
        {
            while (run)
            {
                string zart = phoenixapi.get_message();

                string full_packet = "";
                if (zart != null && zart != "")
                {
                    try
                    {
                        JObject zurna = JObject.Parse(zart);
                        if (((int)zurna["type"]) == ((int)PhoenixApi.Type.packet_send))
                        {
                            full_packet = zurna["packet"].ToString();

                            string[] splitted_packet = full_packet.Split(' ');
                            List<string> packet_splitted = new List<string>();
                            for (int i = 0; i < splitted_packet.Length; i++)
                            {
                                packet_splitted.Add(splitted_packet[i]);
                            }
                            string header = packet_splitted[0];
                            handle_packet(packet_splitted, full_packet);
                        }

                        if (((int)zurna["type"]) == ((int)PhoenixApi.Type.packet_recv))
                        {
                            full_packet = zurna["packet"].ToString();
                            string[] splitted_packet = full_packet.Split(' ');
                            List<string> packet_splitted = new List<string>();
                            for (int i = 0; i < splitted_packet.Length; i++)
                            {
                                packet_splitted.Add(splitted_packet[i]);
                            }
                            string header = packet_splitted[0];
                            handle_packet(packet_splitted, full_packet);
                        }
                        if (((int)zurna["type"]) == ((int)PhoenixApi.Type.query_player_info))
                            player.handle_player_infos(zart);
                        if (((int)zurna["type"]) == ((int)PhoenixApi.Type.query_inventory))
                            player.handle_inventory(zart);
                        if (((int)zurna["type"]) == ((int)PhoenixApi.Type.query_skills_info))
                            player.handle_skills(zart);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message + " " + full_packet);
                    }
                }
                else
                    await Task.Delay(1);
            }
        }

        void handle_packet(List<string> packet_splitted, string full_packet)
        {
            scene.handle_packets(packet_splitted, full_packet);
            player.handle_packets(packet_splitted, full_packet);
            bot.handle_packets(packet_splitted, full_packet);
        }
    }
}
