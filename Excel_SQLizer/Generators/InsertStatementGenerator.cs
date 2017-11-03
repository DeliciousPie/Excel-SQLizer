using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    internal class InsertStatementGenerator : BaseStatementGenerator
    {

        private string _statementPrefix;

        public InsertStatementGenerator(string tableName, string columns)
        {
            _statementPrefix = "INSERT INTO " + tableName + " (" + columns + ") ";
            string fileName = tableName.ToUpper() + "_INSERT_STATEMENTS.sql";
            Initialize(columns, fileName, tableName);
        }

        public override void AddStatement(List<object> values)
        {
            string statement = _statementPrefix + " VALUES (";
            foreach (var value in values)
            {
                statement += value.ToString() + ",";
            }
            statement = statement.TrimEnd(',') + ")";
            _statements.Add(statement);
        }
    }
}
