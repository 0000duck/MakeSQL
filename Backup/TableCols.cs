using System;
using System.Collections.Generic;
using System.Text;

namespace MakeSQL
{
    public class TableCols
    {
        private string colName = string.Empty;

        public string ColName
        {
            get { return colName; }
            set { colName = value; }
        }
        private string dataType = string.Empty;

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        private int dataLength = -1;

        public int DataLength
        {
            get { return dataLength; }
            set { dataLength = value; }
        }
        private string comments = string.Empty;

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
    }
}
