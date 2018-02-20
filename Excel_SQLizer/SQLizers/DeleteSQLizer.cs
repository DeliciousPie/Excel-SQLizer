using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel_SQLizer.Generators;

namespace Excel_SQLizer.SQLizers
{
    public class DeleteSQLizer : BaseSQLizer
    {
        public DeleteSQLizer(FileType fileType, MemoryStream stream, string tableName = null)
        {
            Initialize(fileType, stream, tableName);
        }

        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            return new DeleteStatementGenerator(tableName, columns);
        }
    }
}
