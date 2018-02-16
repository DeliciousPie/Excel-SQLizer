using System;
using System.Collections.Generic;
using System.IO;
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

        public UpdateSQLizer(FileType fileType, MemoryStream stream)
        {
            Initialize(fileType, stream);
        }

        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            return new UpdateStatementGenerator(tableName, columns);
        }
    }
}
