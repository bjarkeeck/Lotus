using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LotusEditor
{
    static class Program
    {
        public static MainWindow MainForm { get; private set; }

        /// <summary>
        /// The main entry point for the application..
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm = new MainWindow();

            Application.Run(MainForm);
        }
    }
}
