using System;
using System.Collections.Generic;
using System.Text;
using Excel_SQLizer.Generators;

namespace Excel_SQLizer.SQLizers
{
    public class UpdateSQLizer : BaseSQLizer
    {
        
        public UpdateSQLizer(string filePath, string outPath = null)
        {
            Initialize(filePath, outPath);
        }

        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            return new UpdateStatementGenerator(tableName, columns);
        }
    }
}
