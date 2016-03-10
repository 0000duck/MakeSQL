using System;
using System.Collections.Generic;
using System.Text;

namespace MakeSQL
{
    public class NFC
    {
        public static string UserID = string.Empty;
        public static string Password = string.Empty;
        public static string DataSource = string.Empty;
        public static string ConnString = "data source={2};password={1};persist security info=True;user id={0}";
    }
}
