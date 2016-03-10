using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace MakeSQL
{
    public class Logo
    {
        public Logo()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            newLogo();
        }
        public Logo(string strFileName)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.strFileName = strFileName;
            newLogo();
        }
        private void newLogo()
        {
            if (!File.Exists(this.strFileName))
            {
                //System.IO.File.Create(this.strFileName);
                //Create the file. 
                using (FileStream fs = File.Create(this.strFileName))
                {
                    //fs.Write("");
                }
            }
        }

        #region 变量
        private string strFileName = ".\\logo.txt";
        private System.IO.TextWriter output;

        private string err = string.Empty;

        public string Err
        {
            get { return err; }
            set { err = value; }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设置/读取文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return strFileName;
            }
            set
            {
                strFileName = value;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="str"></param>
        public void WriteLogo(string str)
        {
            try
            {
                output = File.AppendText(strFileName);
                output.WriteLine(System.DateTime.Now + "\n" + str);
                output.Close();
            }
            catch (Exception ex)
            { 
                err = ex.Message; 
            }
        }

        /// <summary>
        /// 写txt
        /// </summary>
        /// <param name="str"></param>
        public void WriteLine(string str)
        {
            try
            {
                output = File.AppendText(strFileName);
                output.WriteLine(str);
                output.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
        }
        #endregion
    }

}
