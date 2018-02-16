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
        protected string _filePath;
        protected string _outPath;
        protected FileType _fileType;
        protected MemoryStream _stream;
        protected List<BaseStatementGenerator> _statementGenerators;

        /// <summary>
        /// Initializes all SQLizer settings.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="outPath">The out path.</param>
        protected void Initialize(string filePath, string outPath = null)
        {
            _filePath = filePath;
            //Sets an out path for the file if passed in, otherwise default to same path as the excel file
            _outPath = outPath ?? Path.GetDirectoryName(filePath);
            _statementGenerators = new List<BaseStatementGenerator>();
        }

        /// <summary>
        /// Initializes all SQLizer settings.
        /// </summary>
        /// <param name="stream">The stream of the file to be SQLized.</param>
        protected void Initialize(FileType fileType, MemoryStream stream)
        {
            _fileType            = fileType;
            _stream              = stream;
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
        /// Generates the SQL scripts.
        /// </summary>
        /// <exception cref="WorkbookOpenException"></exception>
        public void GenerateSQLScripts()
        {
            try
            {
                using (FileStream stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int tableCount = reader.ResultsCount;

                        do
                        {
                            //first row is the column names
                            string tableName = reader.Name;
                            string columns = "";
                            reader.Read();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columns += reader.GetString(i) + ", ";
                            }
                            //removing trailing comma and space
                            columns = columns.Trim().TrimEnd(',');

                            BaseStatementGenerator generator = CreateGenerator(tableName, columns);

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
                                            //if value is string wrap it in ' ' quotes, else just add it.
                                            var fieldType = reader.GetFieldType(i).Name.ToLower();
                                            if (fieldType.ToString().Equals("string"))
                                            {
                                                vals.Add("'" + reader.GetString(i) + "'");
                                            }
                                            else
                                            {
                                                vals.Add(reader.GetValue(i));
                                            }
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
            catch (IOException)
            {
                throw new WorkbookOpenException();
            }
            catch (Exception e)
            {
                throw e;
            }
            //write out the SQL file
            WriteSqlFile();

        }

        /// <summary>
        /// Writes the SQL file.
        /// </summary>
        internal void WriteSqlFile()
        {
            foreach (BaseStatementGenerator generator in _statementGenerators)
            {
                string filePath = _outPath + @"\" + generator.GetFileName();
                //if file exists, delete it
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                //create a file to write to
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (string insertStatement in generator.GetStatements())
                    {
                        sw.WriteLine(insertStatement);
                    }
                }
            }
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
            // Currently, just checking that the PK column is not null and is not a comment character (e.g. // or --)
            // For this hasty bug fix, just check that it isn't a string. String PKs are not really a thing but still this is 
            //  dumb to not document.
            bool result = !reader.IsDBNull(0) && reader.GetFieldType(0).Name.ToLower() != "string";

            return result;
        }
    }
}
