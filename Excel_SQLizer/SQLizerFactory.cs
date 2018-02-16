using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel_SQLizer.Generators;
using Excel_SQLizer.SQLizers;

namespace Excel_SQLizer
{
    // Defines what type of SQLizer to create - what SQL statements do you want to generate?
    public enum SQLizerType
    {
        Insert,
        Update,
        Delete,
        InsertOrUpdate
    }

    // Controls whether the file being read is an Excel file (e.g. .xlsx) or a CSV 
    public enum FileType
    {
        Excel,
        CSV
    }

    public static class SQLizerFactory
    {
        public static BaseSQLizer Create(SQLizerType option, string filePath, string outPath = null)
        {
            BaseSQLizer sqlizer = null;
            switch (option)
            {
                case SQLizerType.Insert:
                    sqlizer = new InsertSQLizer(filePath, outPath);
                    break;
                case SQLizerType.Update:
                    sqlizer = new UpdateSQLizer(filePath, outPath);
                    break;
                case SQLizerType.Delete:
                    sqlizer = new DeleteSQLizer(filePath, outPath);
                    break;
                case SQLizerType.InsertOrUpdate:
                    sqlizer = new InsertOrUpdateSQLizer(filePath, outPath);
                    break;
                default:
                    throw new Exception("Invalid SQLizer option");
            }

            return sqlizer;
        }

        public static BaseSQLizer Create(SQLizerType sqlType, FileType fileType, MemoryStream stream)
        {
            BaseSQLizer sqlizer = null;
            switch (sqlType)
            {
                case SQLizerType.Insert:
                    sqlizer = new InsertSQLizer(fileType, stream);
                    break;
                case SQLizerType.Update:
                    sqlizer = new UpdateSQLizer(fileType, stream);
                    break;
                case SQLizerType.Delete:
                    sqlizer = new DeleteSQLizer(fileType, stream);
                    break;
                case SQLizerType.InsertOrUpdate:
                    sqlizer = new InsertOrUpdateSQLizer(fileType, stream);
                    break;
                default:
                    throw new Exception("Invalid SQLizer option");
            }

            return sqlizer;
        }
    }

}
