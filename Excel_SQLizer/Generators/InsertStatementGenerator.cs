using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    internal class InsertStatementGenerator : BaseStatementGenerator
    {


        public InsertStatementGenerator(string tableName, string columns)
        {
            string prefix = "INSERT INTO " + tableName + " (" + columns + ") ";
            string fileName = tableName.ToUpper() + "_INSERT_STATEMENTS.sql";
            Initialize(prefix, fileName);
        }

        public override void AddStatement(string values)
        {
            string statement = _statementPrefix + " VALUES (" + values + ")";
            _statements.Add(statement);
        }
    }
}
