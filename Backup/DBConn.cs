using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MakeSQL
{
    public partial class DBConn : Form
    {
        private string filePath = Application.StartupPath + @"\Server.xml";

        public DBConn()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUserID.Text))
            {
                MessageBox.Show("�û�������Ϊ�գ�");
                this.txtUserID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                MessageBox.Show("���벻��Ϊ�գ�");
                this.txtPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtDataSource.Text))
            {
                MessageBox.Show("����Դ����Ϊ�գ�");
                this.txtDataSource.Focus();
                return;
            }

            NFC.UserID = this.txtUserID.Text;
            NFC.Password = this.txtPassword.Text;
            NFC.DataSource = this.txtDataSource.Text;

            NFC.ConnString = string.Format(NFC.ConnString, NFC.UserID, NFC.Password, NFC.DataSource);

            //this.Close();
            XmlManager.UpdateXML(filePath, "UID", NFC.UserID);
            XmlManager.UpdateXML(filePath, "PWD", NFC.Password);
            XmlManager.UpdateXML(filePath, "DataSource", NFC.DataSource);

            MessageBox.Show("����ɹ���");
        }

        private void DBConn_Load(object sender, EventArgs e)
        {
            string userID = string.Empty;
            string password = string.Empty;
            string dataSource = string.Empty;
            XmlManager.ReadNodeValue(filePath, "UID",ref userID);
            XmlManager.ReadNodeValue(filePath, "PWD", ref password);
            XmlManager.ReadNodeValue(filePath, "DataSource", ref dataSource);

            this.txtUserID.Text = userID;
            this.txtPassword.Text = password;
            this.txtDataSource.Text = dataSource;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            OracleDA da = new OracleDA();

            bool ret = da.IsConnection();

            if (ret)
            {
                MessageBox.Show("���ӳɹ���");
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ�");
                return;
            }
        }
    }
}