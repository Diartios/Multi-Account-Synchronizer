using Multi_Account_Synchronizer;
using Multi_Account_Synchronizer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Account_Synchronizer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form1 = new Form1();
            Form2 form2 = new Form2();
            form1.form2 = form2;
            form2.form1 = form1;
            Application.Run(form1);
        }
    }
}
