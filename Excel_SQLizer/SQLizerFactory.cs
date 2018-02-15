using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel_SQLizer.Generators;
using Excel_SQLizer.SQLizers;

namespace Excel_SQLizer
{
    public enum SQLizerOptions
    {
        Insert,
        Update,
        Delete,
        InsertOrUpdate
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
                    sqlizer = new UpdateSQLizer(filePath, outPath);
                    break;
                case SQLizerOptions.Delete:
                    sqlizer = new DeleteSQLizer(filePath, outPath);
                    break;
                case SQLizerOptions.InsertOrUpdate:
                    sqlizer = new InsertOrUpdateSQLizer(filePath, outPath);
                    break;
                default:
                    throw new Exception("Invalid SQLizer option");
            }

            return sqlizer;
        }

        public static BaseSQLizer Create(SQLizerOptions option, MemoryStream stream)
        {
            BaseSQLizer sqlizer = null;
            //switch (option)
            //{
            //    case SQLizerOptions.Insert:
            //        sqlizer = new InsertSQLizer(stream);
            //        break;
            //    case SQLizerOptions.Update:
            //        sqlizer = new UpdateSQLizer(stream);
            //        break;
            //    case SQLizerOptions.Delete:
            //        sqlizer = new DeleteSQLizer(stream);
            //        break;
            //    case SQLizerOptions.InsertOrUpdate:
            //        sqlizer = new InsertOrUpdateSQLizer(stream);
            //        break;
            //    default:
            //        throw new Exception("Invalid SQLizer option");
            //}

            return sqlizer;
        }
    }

}
