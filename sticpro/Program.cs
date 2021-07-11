using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sticpro
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static string id;
        public static string collection;
        public static string price;
        public static string amount;

        public static string IDhis;
        public static string namehis;
        public static string amounthis;
        public static string sumhis;

        public static string IDstock;
        public static string namestock;
        public static string pricestock;
        public static string amountstock;

    }
}

    


