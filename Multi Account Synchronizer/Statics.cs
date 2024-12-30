using ICSharpCode.SharpZipLib.Zip;
using IniParser.Model;
using Multi_Account_Synchronizer.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class Statics
    {
        public static T JsonGetValueOrDefault<T>(JToken jsonObject, string key, T defaultValue)
        {
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
                return (T)Convert.ChangeType(iniData[sectionName][keyName], typeof(T));
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

        public static int[][] LoadMap(int map_id)
        {
            //i've stolen all the code from stradiveri
            byte[] data = null;
            int width = 0;
            int height = 0;
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
            catch (Exception)
            {
                return new int[0][];
            }
            if (data == null)
                return new int[0][];
            #region bullshit if-else copied from stradiveri
            if (data[1] == 0)
            {
                width = data[0];
                height = (data.Length - 4) / data[0];
            }
            else if (data[3] == 0)
            {
                height = data[2];
                width = (data.Length - 4) / data[2];
            }
            else if (data[0] == data[2])
            {
                double sqroot = Math.Sqrt(data.Length - 4);
                width = (int)sqroot;
                height = (int)sqroot;
            }
            else if (data[0] == 19 && data[2] == 14)
            {
                width = 275;
                height = 270;
            }
            else if (data[0] == 204 && data[2] == 9)
            {
                width = 460;
                height = 265;
            }
            else if (data[0] == 54 && data[2] == 34)
            {
                width = 310;
                height = 290;
            }
            else if (data[0] == 54 && data[2] == 44)
            {
                width = 310;
                height = 300;
            }
            else if (data[0] == 94 && data[2] == 145)
            {
                width = 350;
                height = 401;
            }
            else if (data[0] == 4 && data[2] == 24)
            {
                width = 260;
                height = 280;
            }
            else if (data[0] == 16 && data[2] == 10)
            {
                width = 272;
                height = 266;
            }
            else
            {
                Console.WriteLine($"Error while loading map: {map_id}");
                return new int[0][];
            }
            #endregion
            int[][] result = ConvertToArray(data, width, height);
            return result;
        }
        static int[][] ConvertToArray(byte[] data, int width, int height)
        {
            int totalElements = width * height;

            if (data.Length != totalElements)
            {
                throw new ArgumentException("Invalid data length for the given width and height");
            }

            int[][] resultArray = new int[height][];

            for (int i = 0; i < height; i++)
            {
                resultArray[i] = new int[width]; // Initialize each row
                for (int j = 0; j < width; j++)
                {
                    int index = i * width + j;
                    resultArray[i][j] = data[index] != 0 ? 0 : 1;
                }
            }

            return resultArray;
        }
    }
}
