using System;
using System.Collections.Generic;
using System.Text;
using Excel_SQLizer.Generators;

namespace Excel_SQLizer.SQLizers
{
    public class InsertSQLizer : BaseSQLizer
    {

        public InsertSQLizer(string filePath, string outPath = null)
        {
            Initialize(filePath, outPath);
        }


        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns, string wherePrefix)
        {
            return new InsertStatementGenerator(tableName, columns);
        }
    }
}
