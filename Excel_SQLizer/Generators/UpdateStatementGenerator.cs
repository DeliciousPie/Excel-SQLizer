using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    public class UpdateStatementGenerator : BaseStatementGenerator
    {
        public UpdateStatementGenerator(string tableName, string columns)
        {
            string fileName = tableName.ToUpper() + "_UPDATE_STATEMENTS.sql";

            Initialize(columns, fileName, tableName);
        }

        //public override void AddStatement(string values)
        //{
        //    //WHERE col1 = val1, col2= val2 ...
        //    //THIS BREAKS ON STRING FIELDS WITH COMMAS IN THEM
        //    string[] valArray = values.Split(',');
        //    string statement = "UPDATE " + _tableName + " SET ";

        //    for (int i = 0; i < _colArray.Length; i++)
        //    {
        //        statement += _colArray[i] + " = " + valArray[i] + ", ";
        //    }
        //    statement = statement.Trim().TrimEnd(',');
        //    //PK must be first column
        //    statement += " WHERE " + _colArray[0] + " = " + valArray[0];
        //    _statements.Add(statement);
        //}
        public override void AddStatement(List<object> values)
        {
            string statement = "UPDATE " + _tableName + " SET ";
            for (int i = 0; i < _colArray.Length; i++)
            {
                statement += _colArray[i] + " = " + values[i] + ", ";
            }
            statement = statement.Trim().TrimEnd(',');
            //PK must be first column
            statement += " WHERE " + _colArray[0] + " = " + values[0];
            _statements.Add(statement);
        }
    }
}
