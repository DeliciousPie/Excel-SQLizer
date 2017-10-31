using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    public class UpdateStatementGenerator : BaseStatementGenerator
    {
        private string[] _colArray;
        public UpdateStatementGenerator(string tableName, string columns)
        {
            string fileName = tableName.ToUpper() + "_UPDATE_STATEMENTS.sql";
            _colArray = columns.Split(',');
            Initialize(columns, fileName, tableName);
        }

        public override void AddStatement(string values)
        {
            //WHERE col1 = val1, col2= val2 ...
            string[] valArray = values.Split(',');
            string statement = "UPDATE " + _tableName + " SET ";

            for (int i = 0; i < _colArray.Length; i++)
            {
                statement += _colArray[i] + " = " + valArray[i] + ", ";
            }
            statement = statement.Trim().TrimEnd(',');
            //PK must be first column
            statement += " WHERE " + _colArray[0] + " = " + valArray[0];
            _statements.Add(statement);
        }
    }
}
