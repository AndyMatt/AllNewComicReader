using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AllNewComicReader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(args.Count<string>() > 0)
                Application.Run(new Form1(args));
            else
            Application.Run(new Form1());
        }
    }
}
