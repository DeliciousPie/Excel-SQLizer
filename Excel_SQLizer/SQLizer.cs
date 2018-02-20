using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel_SQLizer.Exceptions;
using ExcelDataReader;

namespace Excel_SQLizer
{
    public abstract class BaseSQLizer
    {
        // The type of file being read
        protected FileType _fileType;
        // The memory stream of the file
        protected MemoryStream _stream;
        // Optional, the name of the table. Only used when reading a CSV.
        protected string _tableName;
        // A list of all statement generators created.
        protected List<BaseStatementGenerator> _statementGenerators;

        /// <summary>
        /// Initializes all SQLizer settings.
        /// </summary>
        /// <param name="stream">The stream of the file to be SQLized.</param>
        protected void Initialize(FileType fileType, MemoryStream stream, string tableName = null)
        {
            _fileType            = fileType;
            _stream              = stream;
            _tableName           = tableName;
            _statementGenerators = new List<BaseStatementGenerator>();
        }

        /// <summary>
        /// Creates a generator.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The columns - comma deliminted.</param>
        /// <returns>A BaseStatementGenerator of the correct type</returns>
        protected abstract BaseStatementGenerator CreateGenerator(string tableName, string columns);


        /// <summary>
        /// Gets the SQL statements from the file used to create the SQLizer.
        /// </summary>
        /// <returns>A dictionary keyed on the table name with a value of a List of strings of SQL statements.</returns>
        /// <exception cref="WorkbookOpenException">Workbook is open by another process and cannot be accessed.</exception>
        public Dictionary<string, List<string>> GetSQLStatements()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            try
            {
                // Open the stream
                using (_stream)
                {
                    using (IExcelDataReader reader = GetReader())
                    {
                        // Read each worksheet
                        do
                        {
                            // First row is column names
                            string columns = "";
                            // Moves the reader to the first row of the worksheet
                            reader.Read();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columns += reader.GetString(i) + ", ";
                            }
                            //removing trailing comma and space
                            columns = columns.Trim().TrimEnd(',');
                            // CSV readers don't have access to the sheet name. 
                            string tableName = reader.Name != string.Empty
                                                ? reader.Name
                                                : _tableName;

                            BaseStatementGenerator generator = CreateGenerator(tableName, columns);

                            // Read each row
                            while (reader.Read())
                            {
                                if (ColumnsHaveData(reader))
                                {
                                    List<object> vals = new List<object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        //For null fields use the NULL keyword
                                        if (reader.IsDBNull(i))
                                        {
                                            vals.Add("NULL");
                                        }
                                        else
                                        {
                                            vals.Add(GetReaderValue(reader, i));
                                        }

                                    }
                                    generator.AddStatement(vals);
                                }
                            }
                            _statementGenerators.Add(generator);
                        } while (reader.NextResult());
                    }
                }
            }
            catch (IOException ex)
            {
                throw new WorkbookOpenException("Workbook is open by another process and cannot be accessed.", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            result = BuildResults();

            return result;
        }

        /// <summary>
        /// Builds the results to be returned to the client.
        /// </summary>
        /// <returns>A dictionary keyed on table name with a value of a list of SQL statements for that table.</returns>
        private Dictionary<string, List<string>> BuildResults()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            foreach (BaseStatementGenerator generator in _statementGenerators)
            {
                result.Add(generator.TableName, generator.Statements);
            }

            return result;
        }

        /// <summary>
        /// Gets the reader value at the specified index.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="readerIndex">Index of the reader.</param>
        /// <returns>The value of the reader. It is returned as an object, but it is formatted correctly for the SQL statement.</returns>
        private object GetReaderValue(IExcelDataReader reader, int readerIndex)
        {
            object result = null;
            // CSV data readers always return values as strings
            if (_fileType == FileType.CSV)
            {
                result = GetCSVReaderValue(reader, readerIndex);
            }
            else
            {
                // If the data type is a string, wrap in single quotes
                var fieldType = reader.GetFieldType(readerIndex).Name.ToLower();
                if (fieldType.ToString().Equals("string"))
                {
                    result = $"'{reader.GetString(readerIndex)}'";
                }
                // Else return value as an object
                else
                {
                    result = reader.GetValue(readerIndex);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the CSV reader value. Any value that starts with a ' or " is treated as a string. Numbers as text
        /// are treated as text. We expect numbers to actually be numbers.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="readerIndex">Index of the reader.</param>
        /// <returns></returns>
        private object GetCSVReaderValue(IExcelDataReader reader, int readerIndex)
        {
            object result = reader.GetValue(readerIndex);
            // Strings need to be wrapped in single quotes
            // First, check if it's a number as a text. If it starts with a ' or " we don't care if it'll parse as something else -  it's a string
            if (result.ToString().StartsWith("\"") || result.ToString().StartsWith("'"))
            {
                result = $"'{result.ToString()}'";
            }
            else if (int.TryParse(result.ToString(), out int intResult))
            {
                result = intResult;
            }
            else if (double.TryParse(result.ToString(), out double dblResult))
            {
                result = dblResult;
            }
            else if (DateTime.TryParse(result.ToString(), out DateTime dtResult))
            {
                result = dtResult;
            }
            // Don't insert empty strings - make them NULL
            else if (result.ToString() != string.Empty)
            {
                result = $"'{result.ToString()}'";
            }
            else
            {
                result = "NULL";
            }

            return result;
        }


        /// <summary>
        /// Gets the correct reader for the filetype of the stream.
        /// </summary>
        /// <returns>An IExcelDataReader of the correct type for the _stream.</returns>
        private IExcelDataReader GetReader()
        {
            IExcelDataReader reader = _fileType == FileType.Excel
                                        ? ExcelReaderFactory.CreateReader(_stream)
                                        : ExcelReaderFactory.CreateCsvReader(_stream);

            return reader;
        }

        /// <summary>
        /// Determines if the columns of the current row of the reader have data (e.g. not null and not commented)
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private bool ColumnsHaveData(IExcelDataReader reader)
        {
            bool result = !reader.IsDBNull(0);
            // If it's null, ignore
            if (result)
            {
                // If it's a string that starts with // or -- then we ignore
                // In CSVs all results come back as a string, so check that the length is at least at least 2.
                // If it's any less then it can't possibly be comments
                if (reader.GetFieldType(0).Name.ToLower() == "string" && reader.GetValue(0).ToString().Length >= 2)
                {
                    string firstChars = reader.GetValue(0).ToString().Substring(0,2);
                    result = firstChars != @"//" && firstChars != @"--";
                }
            }

            return result;
        }
    }
}
