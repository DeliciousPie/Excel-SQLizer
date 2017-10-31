using System;
using System.Collections.Generic;
using System.Text;
using Excel_SQLizer.Generators;
using Excel_SQLizer.SQLizers;

namespace Excel_SQLizer
{
    public enum SQLizerOptions
    {
        Insert,
        Update,
        Delete
    }
    public static class SQLizerFactory
    {
        public static BaseSQLizer Create(SQLizerOptions option, string filePath, string outPath = null)
        {
            BaseSQLizer sqlizer = null;
            switch (option)
            {
                case SQLizerOptions.Insert:
                    sqlizer = new InsertSQLizer(filePath, outPath);
                    break;
                case SQLizerOptions.Update:
                    throw new NotImplementedException();
                    break;
                case SQLizerOptions.Delete:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception("Invalid SQLizer option");
            }

            return sqlizer;
        }
    }

}
