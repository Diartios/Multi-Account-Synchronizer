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
            //😎😎😎
            byte[] data = null;
            
            int width = BitConverter.ToUInt16(data, 0);
            int height = BitConverter.ToUInt16(data, 2);
            
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
