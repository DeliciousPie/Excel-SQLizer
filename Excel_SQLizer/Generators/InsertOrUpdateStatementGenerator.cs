using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    public class InsertOrUpdateStatementGenerator : BaseStatementGenerator
    {

        public InsertOrUpdateStatementGenerator(string tableName, string columns)
        {
            string fileName = tableName.ToUpper() + "_INSERT_OR_UPDATE_STATEMENTS.sql";
            Initialize(columns, fileName, tableName);
        }


        public override void AddStatement(List<object> values)
        {
            string statement = "IF EXISTS (SELECT * FROM " + _tableName + " WHERE "
                + _colArray[0] + " = " + values[0] + ")\n" + "BEGIN\n\t";
            //Build Update statement
            statement += "UPDATE " + _tableName + " SET ";
            for (int i = 0; i < _colArray.Length; i++)
            {
                statement += _colArray[i] + " = " + values[i] + ", ";
            }
            statement = statement.Trim().TrimEnd(',');
            statement += " WHERE " + _colArray[0] + " = " + values[0];
            //close if and start else
            statement += "\nEND\nELSE\nBEGIN\n\t";
            //Build insert statement
            statement += "INSERT INTO " + _tableName + " (";
            //add column names
            foreach (string column in _colArray)
            {
                statement += column + ", ";
            }
            statement = statement.Trim().TrimEnd(',') + ")";
            //add values
            statement += " VALUES (";
            foreach (var value in values)
            {
                statement += value.ToString() + ",";
            }
            statement = statement.TrimEnd(',') + ")\n";
            statement += "END";

            _statements.Add(statement);
        }
    }
}
