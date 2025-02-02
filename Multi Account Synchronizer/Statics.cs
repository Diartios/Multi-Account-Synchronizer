using ICSharpCode.SharpZipLib.Zip;
using IniParser.Model;
using Multi_Account_Synchronizer.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class Statics
    {
        public static Dictionary<string,string> InviteCommands = new Dictionary<string, string>()
        {
            {"Invite", "EN-ES-CZ-PL" },
            {"Einladung","DE" },
            {"Inviter", "FR" },
            {"Davet", "TR" },
            {"InvM", "IT" }
        };
        public static T JsonGetValueOrDefault<T>(JToken jsonObject, string key, T defaultValue)
        {
            if (jsonObject == null)
                return defaultValue;
            try
            {
                return jsonObject[key].Value<T>();
            }
            catch
                
            {
                return defaultValue;
            }
        }

        public static T IniGetValueOrDefault<T>(IniData iniData, string sectionName, string keyName, T defaultValue)
        {
            try
            {

                string value = iniData[sectionName][keyName];
                //in case decimal symbol is not "." 
                if (typeof(T) == typeof(double) && CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                {
                   
                    value = value.Replace(".", ",");
                }
                var resultValue = (T)Convert.ChangeType(value, typeof(T));
                return resultValue;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return defaultValue;
            }
        }


        public static double Distance(Point p1, Point p2)
        {
            double distance = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            return distance;
        }
        
        public static bool Chance(int percentage)
        {
            if (percentage <= 0)
                return false;
            if (percentage >= 100) return true;
            Random rnd = new Random();
            return rnd.Next(0, 100) < percentage;
        }

        public static int[][] LoadMap(int map_id)
        {
            //i've stolen all the code from stradiveri
            byte[] data = null;

            try
            {
                FileStream fs = File.OpenRead("maps.zip");
                ZipFile zipfile = new ZipFile(fs);
                ZipEntry entry = zipfile.GetEntry($"{map_id}.bin");
                if (entry != null)
                {
                    using (Stream zipStream = zipfile.GetInputStream(entry))
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        zipStream.CopyTo(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception e)   
            {
                Console.WriteLine(e.Message);
                return new int[0][];
            }
            if (data == null)
            {
                Console.WriteLine($"data null map id {map_id}");
                return new int[0][];
            }
                

            int width = BitConverter.ToUInt16(data, 0);
            int height = BitConverter.ToUInt16(data, 2);

            int[][] result = ConvertToArray(data.Skip(4).ToArray(), width, height);
            return result;
        }
        static int[][] ConvertToArray(byte[] data, int width, int height)
        {
            int totalElements = width * height;

            if (data.Length != totalElements)
            {
                throw new ArgumentException("Invalid data length for the given width and height");
            }

            int[][] resultArray = new int[width][]; 

            for (int i = 0; i < width; i++)
            {
                resultArray[i] = new int[height]; 
                for (int j = 0; j < height; j++)
                {
                    int index = j * width + i; 
                    resultArray[i][j] = data[index] != 0 ? 0 : 1;
                }
            }

            return resultArray;
        }
    }
}
