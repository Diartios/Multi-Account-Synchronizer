using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class PortFinder
    {
        List<string> windowTexts = new List<string>();
        StringBuilder windowText = new StringBuilder(256);
        public int fafnirbotcount = 0;
        public List<string> find_ports()
        {
            fafnirbotcount = 0;
            List<string> ports = new List<string>();
            windowText.Clear();
            windowTexts.Clear();

            EnumWindows(EnumProc, IntPtr.Zero);
            foreach (string windowText in windowTexts)
            {
                if (windowText.Contains("] - Phoenix Bot:"))
                {
                    string[] names = windowText.Split(' ');
                    string[] port = windowText.Split(':');
                    if (names[2].Replace("]", "") != "")
                        ports.Add(names[2].Replace("]", "") + " " + port[1]);
                    else
                        ports.Add(port[1]);
                }
                if ((windowText == "Fafnir Bot" || windowText.Contains("Fafnir Bot - ")) && !windowText.Contains("Microsoft"))
                    fafnirbotcount++;
            }
            if (fafnirbotcount == 0)
                fafnirbotcount = 1;
            return ports;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


        bool EnumProc(IntPtr hWnd, IntPtr lParam)
        {

            GetWindowText(hWnd, windowText, windowText.Capacity);
            windowTexts.Add(windowText.ToString());
            return true;
        }


    }
}
