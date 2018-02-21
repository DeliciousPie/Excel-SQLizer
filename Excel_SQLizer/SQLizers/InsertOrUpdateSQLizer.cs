using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel_SQLizer.Generators;

namespace Excel_SQLizer.SQLizers
{
    internal class InsertOrUpdateSQLizer : BaseSQLizer
    {
        public InsertOrUpdateSQLizer(FileType fileType, MemoryStream stream, string tableName = null)
        {
            Initialize(fileType, stream, tableName);
        }

        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            return new InsertOrUpdateStatementGenerator(tableName, columns);
        }
    }
}
