using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.SQLizers
{
    public class InsertSQLizer : BaseSQLizer
    {

        public InsertSQLizer(string filePath, string outPath = null)
        {

        }


        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            throw new NotImplementedException();
        }
    }
}
