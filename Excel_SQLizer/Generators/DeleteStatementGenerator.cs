using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Generators
{
    public class DeleteStatementGenerator : BaseStatementGenerator
    {
        //TODO: Move this into BaseStatementGenerator
        private string[] _colArray;
        public DeleteStatementGenerator(string tableName, string columns)
        {
            string fileName = tableName.ToUpper() + "_DELETE_STATEMENTS.sql";
            _colArray = columns.Split(',');
            Initialize(columns, fileName, tableName);
        }

        public override void AddStatement(string values)
        {
            
        }
    }
}
