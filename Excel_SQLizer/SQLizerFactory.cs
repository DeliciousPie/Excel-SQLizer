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
        /// <summary>
        /// Creates the specified SQL type.
        /// </summary>
        /// <param name="sqlType">Type of the SQL to generate.</param>
        /// <param name="fileType">Type of the file (CSV or XLSX).</param>
        /// <param name="stream">The stream of the file.</param>
        /// <param name="tableName">
        /// Name of the table. 
        /// Only necessary for CSV files since SQLizer doesn't have access to it's name. Will cause an exception
        /// if this parameter is null and the file type is CSV.
        /// </param>
        /// <returns>A SQLizer of the correct type based on the parameters supplied</returns>
        /// <exception cref="Exception">Invalid SQLizer option</exception>
        /// /// <exception cref="Exception">The tablename parameter is required when reading CSV files!</exception>
        public static ISQLizer Create(SQLizerType sqlType, FileType fileType, MemoryStream stream, string tableName = null)
        {
            // CSVs require that a table name be passed in. The sqlizer will not work correctly without it
            if (fileType == FileType.CSV && string.IsNullOrEmpty(tableName))
            {
                throw new Exception("The tablename parameter is required when reading CSV files!");
            }

            ISQLizer sqlizer = null;
            switch (sqlType)
            {
                case SQLizerType.Insert:
                    sqlizer = new InsertSQLizer(fileType, stream, tableName);
                    break;
                case SQLizerType.Update:
                    sqlizer = new UpdateSQLizer(fileType, stream, tableName);
                    break;
                case SQLizerType.Delete:
                    sqlizer = new DeleteSQLizer(fileType, stream, tableName);
                    break;
                case SQLizerType.InsertOrUpdate:
                    sqlizer = new InsertOrUpdateSQLizer(fileType, stream, tableName);
                    break;
                default:
                    throw new Exception("Invalid SQLizer option.");
            }

            return sqlizer;
        }
    }

}
