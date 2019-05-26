using System;
using System.Windows.Forms;
using CaptchaRecogition;

namespace RECAPTCHA
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new gp_down());
        }
    }
}
