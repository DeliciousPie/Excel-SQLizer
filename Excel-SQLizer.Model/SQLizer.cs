using System;
using System.IO;
using Excel_SQLizer.Model.Exceptions;
using ExcelDataReader;
using System.Collections.Generic;

namespace Excel_SQLizer.Model
{
    public class SQLizer
    { 
        private string _filePath;
        private string _outPath;
        private List<InsertStatementGenerator> _isgList;

        public SQLizer (string filePath)
        {
            _filePath = filePath;
            _outPath  = Path.GetDirectoryName(filePath);
            _isgList = new List<InsertStatementGenerator>();
        }

        public void GenerateInsertScript()
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

                            InsertStatementGenerator isg = new InsertStatementGenerator(tableName, columns);

                            while (reader.Read())
                            {
                                string values = "";
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    //For null fields use the NULL keyword
                                    if (reader.IsDBNull(i))
                                    {
                                        values += "NULL";
                                    }
                                    else
                                    {
                                        //if value is string wrap it in ' ' quotes, else just add it.
                                        var fieldType = reader.GetFieldType(i).Name.ToLower();
                                        if (fieldType.ToString().Equals("string"))
                                        {
                                            values += "'" + reader.GetString(i) + "'";
                                        }
                                        else
                                        {
                                            values += reader.GetValue(i);
                                        }
                                    }

                                    values += ", ";
                                }
                                values = values.Trim().TrimEnd(',');
                                isg.AddInsertStatement(values);
                            }
                            _isgList.Add(isg);

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

        private void WriteSqlFile()
        {
            foreach (InsertStatementGenerator isg in _isgList)
            {
                string filePath = _outPath + @"\" + isg.TableName.ToUpper() + "_INSERT_STATEMENTS.sql";
                //if file exists, delete it
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                //create a file to write to
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (string insertStatement in isg.InsertStatements)
                    {
                        sw.WriteLine(insertStatement);
                    }
                }
            }
        }
    }
}
