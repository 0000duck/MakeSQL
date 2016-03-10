using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MakeSQL
{
    public partial class frmCondition : Form
    {
        /// <summary>
        /// 数据表
        /// </summary>
        DataTable dt = new DataTable();
        
        /// <summary>
        /// 列
        /// </summary>
        private List<TableCols> tcList = new List<TableCols>();
        /// <summary>
        /// 列
        /// </summary>
        public List<TableCols> TcList
        {
            get { return tcList; }
            set 
            { 
                tcList = value;
                AddData2TV();
            }
        }

        public delegate void MyDelegate(string where);
        public event MyDelegate OK;

        public frmCondition()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string where = string.Empty;

            string preWord = string.Empty;
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Cells[0].Value == null)
                {
                    break;
                }

                string word = this.dataGridView1.Rows[i].Cells[0].Value.ToString().ToLower();//字段
                string sym = this.dataGridView1.Rows[i].Cells[2].Value.ToString();//符号
                string myValue =this.dataGridView1.Rows[i].Cells[3].Value.ToString();//值

                string dataType = this.dataGridView1.Rows[i].Cells[1].Value.ToString();

                if (sym == "like")
                {
                    if (dataType == "VARCHAR2" || dataType == "NUMBER")
                    {
                        myValue = "'%" + myValue + "%'";
                    }                    
                }
                else
                {
                    if (dataType == "VARCHAR2" )
                    {
                        myValue = "'" + myValue + "'";
                    }
                    else if (dataType == "NUMBER")
                    {
                        //myValue = myValue;
                    }
                    else if (dataType == "DATE")
                    {
                        myValue = "to_date('" + myValue + "', 'yyyy-mm-dd hh24:mi:ss')";
                    }
                }

                if (string.IsNullOrEmpty(where))
                {
                    where = " where " + word + " " + sym + " " + myValue;

                    if (this.dataGridView1.Rows[i].Cells[4].Value != null)
                    {
                        preWord = this.dataGridView1.Rows[i].Cells[4].Value.ToString();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(preWord)) 
                    {
                        MessageBox.Show("您的sql不合法！");
                        return;
                    }

                    where = where + " " + preWord + " " + word + " " + sym + " " + myValue;
                }
            }

            if (OK != null)
            {
                OK(where);
            }

            this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDatable()
        {
            //新建列
            DataColumn col1 = new DataColumn("列名", typeof(string));
            DataColumn col2 = new DataColumn("类型", typeof(string));
            DataColumn col3 = new DataColumn("条件值", typeof(string));
            
            //添加列
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);

            this.dataGridView1.DataSource = dt.DefaultView;
            this.dataGridView1.Columns[0].ReadOnly = true;

            DataGridViewComboBoxColumn comboxColumn = CreateComboBoxColumn("值关系", 6);
            comboxColumn.Items.AddRange("=", ">", "<", "like", ">=", "<=");
            dataGridView1.Columns.Insert(2,comboxColumn);

            DataGridViewComboBoxColumn comboxColumn2 = CreateComboBoxColumn("字段关系", 2);
            comboxColumn2.Items.AddRange("and", "or");
            dataGridView1.Columns.Add(comboxColumn2);
        }

        private DataGridViewComboBoxColumn CreateComboBoxColumn(string headerName, int num)
        {
            DataGridViewComboBoxColumn column =
                new DataGridViewComboBoxColumn();
            {
                column.DataPropertyName = headerName;
                column.HeaderText = headerName;
                column.DropDownWidth = 160;
                column.Width = 90;
                column.MaxDropDownItems = num;
                column.FlatStyle = FlatStyle.Flat;
            }
            return column;
        }

        private void frmCondition_Load(object sender, EventArgs e)
        {
            InitDatable();
        }

        /// <summary>
        /// 增加数据到节点
        /// </summary>
        private void AddData2TV()
        {
            this.treeView1.Nodes.Clear();

            foreach (TableCols tc in tcList)
            {
                TreeNode tn = new TreeNode(tc.ColName);
                tn.Tag = tc;
                this.treeView1.Nodes.Add(tn);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;

            TableCols tc = tn.Tag as TableCols;

            if (tc != null)
            {
                AddDataDG(tc);
            }
        }

        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="tc"></param>
        private void AddDataDG(TableCols tc)
        {
            if (IsAdded(tc.ColName)) return;

            DataRow dr = dt.NewRow();
            dr["列名"] = tc.ColName;
            dr["类型"] = tc.DataType;

            dt.Rows.Add(dr);
        }

        /// <summary>
        /// 判断是否已经增加
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private bool IsAdded(string colName)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["列名"].ToString() == colName)
                {
                    return true;
                }
            }

            return false;
        }

        private void tsMenuDelete_Click(object sender, EventArgs e)
        {
            bool isDeleted = false;
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Selected == true)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (this.dataGridView1.Rows[i].Cells[0].Value != null && this.dataGridView1.Rows[i].Cells[0].Value.ToString() == dr["列名"].ToString())
                        {
                            dt.Rows.Remove(dr);
                            isDeleted = true;
                            break;
                        }
                    }

                    break;
                }
            }

            if (isDeleted)
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                MessageBox.Show("请选择数据！");
            }
        }
    }
}
