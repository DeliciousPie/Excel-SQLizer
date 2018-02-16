using System;
using System.Collections.Generic;
using System.IO;
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

        public InsertSQLizer(FileType fileType, MemoryStream stream)
        {
            Initialize(fileType, stream);
        }


        protected override BaseStatementGenerator CreateGenerator(string tableName, string columns)
        {
            return new InsertStatementGenerator(tableName, columns);
        }
    }
}
