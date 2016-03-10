using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;

namespace MakeSQL
{
    public class OracleDA
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        private string err = string.Empty;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Err
        {
            get { return err; }
            set { err = value; }
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        private string connString = string.Empty;

        public OracleDA()
        {
            this.connString = NFC.ConnString;
        }

        /// <summary>
        /// �Ƿ����ӷ�����
        /// </summary>
        /// <returns></returns>
        public bool IsConnection()
        {
            bool result = false;
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.err = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// ��ȡ����Ϣ
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<TableCols> GetTableCols(string tableName)
        {
            List<TableCols> list = new List<TableCols>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    conn.Open();

                    OracleCommand cmd = conn.CreateCommand();

                    string sql = @"select tc.column_name as column_name,
                                       tc.data_type as data_type,
                                       tc.data_length as data_length,
                                       cc.comments as comments
                                  from user_tab_columns tc, user_col_comments cc
                                 where tc.table_name = cc.table_name
                                   and tc.column_name = cc.column_name
                                   and tc.table_name = '{0}'
                                    order by tc.column_id";
                    sql = string.Format(sql, tableName);

                    cmd.CommandText = sql;

                    OracleDataReader dr = cmd.ExecuteReader();
                    TableCols tc = null;
                    while (dr.Read())
                    {
                        tc = new TableCols();

                        tc.ColName = dr["column_name"].ToString();
                        tc.DataType = dr["data_type"].ToString();
                        tc.DataLength = Convert.ToInt32(dr["data_length"].ToString());
                        tc.Comments = dr["comments"].ToString();

                        list.Add(tc);
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                this.err = ex.Message;
                return null;
            }

            return list;
        }

    }
}
