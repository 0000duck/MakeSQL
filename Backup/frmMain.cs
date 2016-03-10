using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace MakeSQL
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// 数据存取
        /// </summary>
        private OracleDA dataAccess = new OracleDA();

        /// <summary>
        /// 数据表
        /// </summary>
        private DataTable dt = new DataTable();

        /// <summary>
        /// 生成sql语句
        /// </summary>
        private string geneticSql = string.Empty;

        /// <summary>
        /// 列数据
        /// </summary>
        List<TableCols> list = new List<TableCols>();

        /// <summary>
        /// 条件句子
        /// </summary>
        private string myWhere = string.Empty;

        /// <summary>
        /// 生成sql业务类
        /// </summary>
        private GenerateSQL generateSQL = new GenerateSQL();

        public frmMain()
        {
            InitializeComponent();
            skinEngine1.SkinFile = Application.StartupPath + @"\皮肤\office2007\office2007.ssk";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTableName.Text))
            {
                MessageBox.Show("请输入要查找的数据表！！！");
                return;
            }

            if (!isSelectedField())
            {
                MessageBox.Show("请选择字段！！！");
                this.tabControl1.SelectedIndex = 0;
                return;
            }

            generateSQL.TableName = this.txtTableName.Text.Trim().ToLower();
            generateSQL.RowTable = dt;

            fastColoredTextBox1.AppendText(generateSQL.GenerateInsertSQL());            


            this.tabControl1.SelectedIndex = 1;
            MessageBox.Show("insert sql生成成功！");
        }

       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTableName.Text))
            {
                MessageBox.Show("请输入要查找的数据表！！！");
                return;
            }

            if (!isSelectedField())
            {
                MessageBox.Show("请选择字段！！！");
                this.tabControl1.SelectedIndex = 0;
                return;
            }

            generateSQL.TableName = this.txtTableName.Text.Trim().ToLower();
            generateSQL.RowTable = dt;

            fastColoredTextBox1.AppendText(generateSQL.GenerateUpdateSQL());            

            DialogResult dialogResult = MessageBox.Show("是否需要增加条件？", "提示", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                myWhere = string.Empty;
                frmCondition condition = new frmCondition();
                condition.OK += new frmCondition.MyDelegate(condition_OK);
                condition.TcList = list;
                if (condition.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(myWhere))
                    {
                        fastColoredTextBox1.AppendText(myWhere);
                    }
                }
                else
                {
                    MessageBox.Show("您已经取消！");
                }

                condition.Dispose();
            }

            this.tabControl1.SelectedIndex = 1;
            MessageBox.Show("update sql生成成功！");
        }

        private void btnCondition_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTableName.Text))
            {
                MessageBox.Show("请输入要查找的数据表！！！");
                return;
            }

            if (!isSelectedField())
            {
                MessageBox.Show("请选择字段！！！");
                this.tabControl1.SelectedIndex = 0;
                return;
            }

            generateSQL.TableName = this.txtTableName.Text.Trim().ToLower();
            generateSQL.RowTable = dt;

            fastColoredTextBox1.AppendText(generateSQL.GenerateSelectSQL());

            DialogResult dialogResult = MessageBox.Show("是否需要增加条件？", "提示", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                myWhere = string.Empty;
                frmCondition condition = new frmCondition();
                condition.OK += new frmCondition.MyDelegate(condition_OK);
                condition.TcList = list;
                if (condition.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(myWhere))
                    {
                        this.fastColoredTextBox1.AppendText(myWhere +"\n");
                    }
                }
                else
                {
                    MessageBox.Show("您已经取消！");
                }

                condition.Dispose();
            }

            this.tabControl1.SelectedIndex = 1;
            MessageBox.Show("select sql生成成功！");
        }

        private void condition_OK(string where)
        {
            myWhere = where;
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            DBConn conn = new DBConn();
            conn.Show();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTableName.Text))
            {
                MessageBox.Show("请输入要查找的数据表！！！");
                return;
            }

            //清除所有的sql
            this.fastColoredTextBox1.Clear();

            //去掉空格之后 大写
            string tableName = this.txtTableName.Text.Trim().ToUpper();

            list = dataAccess.GetTableCols(tableName);

            if (list == null)
            {
                MessageBox.Show(dataAccess.Err);
                return;
            }

            if (list.Count <= 0)
            {
                MessageBox.Show("没有您所查找的数据表的数据列！");
                return;
            }

            AddDataDG(list);
            SetWith();

            this.tabControl1.SelectedIndex = 0;
            MessageBox.Show("查找成功！");
        }

        private void AddDataDG(List<TableCols> list)
        {
            dt.Rows.Clear();

            foreach (TableCols tc in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = tc.ColName;
                dr[1] = tc.DataType;
                dr[2] = tc.DataLength;
                dr[3] = tc.Comments;
                dr[4] = false;
                
                this.dt.Rows.Add(dr);
            }
        }

        private void SetWith()
        {
            this.dataGridView1.Columns[0].Width = 150;
            this.dataGridView1.Columns[1].Width = 65;
            this.dataGridView1.Columns[2].Width = 40;
            this.dataGridView1.Columns[3].Width = 250;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitDatable();
        }

        private void InitDatable()
        {
            //新建列
            DataColumn col1 = new DataColumn("列名", typeof(string));
            DataColumn col2 = new DataColumn("类型", typeof(string));
            DataColumn col3 = new DataColumn("精度", typeof(string));
            DataColumn col4 = new DataColumn("注释", typeof(string));
            DataColumn col5 = new DataColumn("选择", typeof(bool));

            //添加列
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);

            this.dataGridView1.DataSource = dt.DefaultView;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr[4] = true;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr[4] = false;
            }
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="queryInfo"></param>
        private void Filter(string queryInfo)
        {
            string filterStr = "列名 like '{0}' or 类型 like '{0}'";

            if (string.IsNullOrEmpty(queryInfo))
            {
                filterStr = " 1=1 ";
            }
            else
            {
                filterStr = string.Format(filterStr, queryInfo);
            }

            this.dt.DefaultView.RowFilter = filterStr;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = "%" + txtFilter.Text + "%";

            Filter(filter);
        }

        private void fastColoredTextBox1_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            FastColoredTextBox tb = (FastColoredTextBox)sender;

            //highlight sql
            tb.SyntaxHighlighter.InitStyleSchema(Language.SQL);
            tb.SyntaxHighlighter.SQLSyntaxHighlight(tb.Range);
            tb.Range.ClearFoldingMarkers();
        }

        /// <summary>
        /// 是否选择了列
        /// </summary>
        /// <returns></returns>
        private bool isSelectedField()
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private void tsMenuClear_Click(object sender, EventArgs e)
        {
            this.fastColoredTextBox1.Clear();
        }

        private void tsMenuCopy_Click(object sender, EventArgs e)
        {
            string result = this.fastColoredTextBox1.SelectedText;

            if (string.IsNullOrEmpty(result)) return;

            Clipboard.SetText(result);
            MessageBox.Show("已复制到剪贴板！！！");
        }

        private void tsMenuSelectAll_Click(object sender, EventArgs e)
        {
            this.fastColoredTextBox1.SelectAll();
        }
    }
}