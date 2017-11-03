using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    public class DeleteStatementGenerator : BaseStatementGenerator
    {
        public DeleteStatementGenerator(string tableName, string columns)
        {
            string fileName = tableName.ToUpper() + "_DELETE_STATEMENTS.sql";
            Initialize(columns, fileName, tableName);
        }

        public override void AddStatement(List<object> values)
        {
            //first value must be ID
            string id = values[0].ToString();
            string statement = "DELETE " + _tableName + " WHERE " + _colArray[0] + " = " + id;
            _statements.Add(statement);
        }
    }
}
