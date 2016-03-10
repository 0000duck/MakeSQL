using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MakeSQL
{
    public class GenerateSQL
    {
        /// <summary>
        /// 完整的sql
        /// </summary>
        private string completeSql = string.Empty;

        /// <summary>
        /// 完整的sql
        /// </summary>
        public string CompleteSql
        {
            get { return completeSql; }
            set { completeSql = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        private string tableName = string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }


        /// <summary>
        /// 行表
        /// </summary>
        private DataTable rowTable = null;
        /// <summary>
        /// 行表
        /// </summary>
        public DataTable RowTable
        {
            get { return rowTable; }
            set { rowTable = value; }
        }

        /// <summary>
        /// 生成查询
        /// </summary>
        /// <returns></returns>
        public string GenerateSelectSQL()
        {
            completeSql = string.Empty;
            string blank = "       ";
            string end = "  from {0}\n";
            end = string.Format(end, tableName.ToLower());

            int colCount = 0;
            int selectedCount = 0;
            int count = 0;

            foreach (DataRow dr in rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    selectedCount++;
                }
            }

            foreach (DataRow dr in rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    string field = dr[0].ToString();
                    string comments = dr[3].ToString();

                    if (colCount == rowTable.Rows.Count - 1 || count + 1 == selectedCount)
                    {
                        completeSql += GetFieldInfoSelect(blank, field, comments, SQLPart.End);
                    }
                    else if (colCount == 0)
                    {
                        completeSql = GetFieldInfoSelect(blank, field, comments, SQLPart.Head);
                    }
                    else
                    {
                        completeSql += GetFieldInfoSelect(blank, field, comments, SQLPart.Body);
                    }

                    count++;
                }

                colCount++;
            }

            completeSql += end;

            return completeSql;
        }

        /// <summary>
        /// 处理字段内容
        /// </summary>
        /// <param name="blank"></param>
        /// <param name="field"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        private string GetFieldInfoSelect(string blank, string field, string comments, SQLPart sqlPart)
        {
            string rowInfo = string.Empty;

            if (sqlPart == SQLPart.Body)
            {
                if (!string.IsNullOrEmpty(comments))
                {
                    rowInfo = blank + field.ToLower() + ", --" + comments;
                }
                else
                {
                    rowInfo = blank + field.ToLower() + ", ";
                }
            }
            else if (sqlPart == SQLPart.Head)
            {
                if (!string.IsNullOrEmpty(comments))
                {
                    rowInfo = "select " + field.ToLower() + ", --" + comments;
                }
                else
                {
                    rowInfo = "select " + field.ToLower() + ", ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(comments))
                {
                    rowInfo = blank + field.ToLower() + " --" + comments;
                }
                else
                {
                    rowInfo = blank + field.ToLower() + " ";
                }
            }

            return rowInfo + "\n";
        }

        public enum SQLPart
        {
            Head,
            Body,
            End
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        /// <returns></returns>
        public string GenerateUpdateSQL()
        {
            completeSql = string.Empty;

            string header = "update {0} \n";
            header = string.Format(header, tableName.ToLower());
            completeSql = header;
            string blank = "       ";


            int colCount = 0;
            int count = 0;
            int selectedCount = 0;

            foreach (DataRow dr in rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    selectedCount++;
                }
            }

            foreach (DataRow dr in rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    string field = dr[0].ToString().ToLower();
                    string dataType = dr[1].ToString();
                    string comments = dr[3].ToString();

                    if (colCount == rowTable.Rows.Count - 1 || count + 1 == selectedCount)
                    {
                        completeSql += blank + field + " = " + GetValueString(dataType, count, comments, true);
                        break;
                    }
                    else if (colCount == 0)
                    {
                        completeSql += "   set " + field + " = " + GetValueString(dataType, count, comments, false);
                    }
                    else
                    {
                        completeSql += blank + field + " = " + GetValueString(dataType, count, comments, false);
                    }

                    count++;
                }

                colCount++;
            }

            return completeSql;
        }

        /// <summary>
        /// 获取值字符串
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="count">计数</param>
        /// <param name="comments">注释</param>
        /// <returns></returns>
        private string GetValueString(string type, int count, string comments, bool endFlag)
        {
            string sqlRowValue = string.Empty;

            bool haveComments = false;
            if (!string.IsNullOrEmpty(comments))
            {
                haveComments = true;
            }

            if (endFlag)
            {
                if (type == "VARCHAR2")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "'{" + count.ToString() + "}'--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "'{" + count.ToString() + "}'";
                    }
                }
                else if (type == "NUMBER")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "{" + count.ToString() + "}--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "{" + count.ToString() + "}";
                    }
                }
                else if (type == "DATE")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "to_date('{" + count.ToString() + "}', 'yyyy-mm-dd hh24:mi:ss')--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "to_date('{" + count.ToString() + "}', 'yyyy-mm-dd hh24:mi:ss')";
                    }
                }
                else
                {
                    if (haveComments)
                    {
                        sqlRowValue = "'{" + count.ToString() + "}'--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "'{" + count.ToString() + "}'";
                    }
                }
            }
            else
            {
                if (type == "VARCHAR2")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "'{" + count.ToString() + "}',--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "'{" + count.ToString() + "}',";
                    }
                }
                else if (type == "NUMBER")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "{" + count.ToString() + "},--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "{" + count.ToString() + "},";
                    }
                }
                else if (type == "DATE")
                {
                    if (haveComments)
                    {
                        sqlRowValue = "to_date('{" + count.ToString() + "}', 'yyyy-mm-dd hh24:mi:ss'),--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "to_date('{" + count.ToString() + "}', 'yyyy-mm-dd hh24:mi:ss'),";
                    }
                }
                else
                {
                    if (haveComments)
                    {
                        sqlRowValue = "'{" + count.ToString() + "}',--" + comments;
                    }
                    else
                    {
                        sqlRowValue = "'{" + count.ToString() + "}',";
                    }
                }
            }

            sqlRowValue += "\n";

            return sqlRowValue;
        }

        /// <summary>
        /// 插入语句
        /// </summary>
        /// <returns></returns>
        public string GenerateInsertSQL()
        {
            completeSql = string.Empty;

            string head = "insert into {0}\n";
            //头部已经构造好了
            head = string.Format(head, tableName);

            completeSql = head;

            string center = "values\n";

            string tail = ")\n";

            //格式化 pl/sql dev
            string blank = "   ";

            //值集合
            List<string> valuesList = new List<string>();

            int count = 0;
            int colCount = 0;
            int selectedCount = 0;

            foreach (DataRow dr in rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    selectedCount++;
                }
            }

            //字段类型
            string colType = string.Empty;
            //注释信息
            string comments = string.Empty;
            //sql行
            string sqlColValue = string.Empty;

            foreach (DataRow dr in this.rowTable.Rows)
            {
                if (Convert.ToBoolean(dr[4]) == true)
                {
                    string colTemp = "{0},";

                    if (colCount == rowTable.Rows.Count - 1 || count + 1 == selectedCount)
                    {
                        colTemp = "{0}";
                    }

                    if (count == 0)
                    {
                        colTemp = "({0},";
                    }

                    colTemp = string.Format(colTemp, dr[0]);
                    if (count == 0)
                    {
                        colTemp = blank.Substring(0, blank.Length - 1) + colTemp;
                    }
                    else
                    {
                        colTemp = blank + colTemp;
                    }

                    //写字段 小写 我超级喜欢小写
                    if (!string.IsNullOrEmpty(dr[3].ToString()))
                    {
                        this.completeSql += colTemp.ToLower() + "--" + dr[3].ToString() + "\n";
                    }
                    else
                    {
                        this.completeSql += colTemp.ToLower() + "\n";
                    }

                    colType = dr[1].ToString();
                    comments = dr[3].ToString();

                    //插入值
                    sqlColValue = GetValueString(colType, count, comments, false);
                    if (colCount == rowTable.Rows.Count - 1 || count + 1 == selectedCount)
                    {
                        int length = sqlColValue.Length;

                        if (string.IsNullOrEmpty(comments))
                        {
                            sqlColValue = sqlColValue.Substring(0, length - 1);
                        }
                        else
                        {
                            int indexComments = sqlColValue.IndexOf("--");
                            string preSql = sqlColValue.Substring(0, indexComments - 1);
                            string postSql = sqlColValue.Substring(indexComments);
                            sqlColValue = preSql + " " + postSql;
                        }
                    }

                    if (count == 0)
                    {
                        sqlColValue = "(" + sqlColValue;
                        sqlColValue = blank.Substring(0, blank.Length - 1) + sqlColValue;
                    }
                    else
                    {
                        sqlColValue = blank + sqlColValue;
                    }

                    valuesList.Add(sqlColValue);

                    count++;
                }

                colCount++;
            }

            this.completeSql +=blank + ")\n";

            this.completeSql +=center;

            for (int i = 0; i < valuesList.Count; i++)
            {
                this.completeSql += valuesList[i];
            }

            this.completeSql += blank + tail;

            return this.completeSql;
        }
    }
}
