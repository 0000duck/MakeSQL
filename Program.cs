using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MakeSQL
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

            GetDB();

            Application.Run(new frmMain());
        }

        static void GetDB()
        {
            string filePath = Application.StartupPath + @"\Server.xml";
            string userID = string.Empty;
            string password = string.Empty;
            string dataSource = string.Empty;
            XmlManager.ReadNodeValue(filePath, "UID", ref userID);
            XmlManager.ReadNodeValue(filePath, "PWD", ref password);
            XmlManager.ReadNodeValue(filePath, "DataSource", ref dataSource);

            NFC.UserID = userID;
            NFC.Password = password;
            NFC.DataSource = dataSource;

            NFC.ConnString = string.Format(NFC.ConnString, NFC.UserID, NFC.Password, NFC.DataSource);
        }
    }
}