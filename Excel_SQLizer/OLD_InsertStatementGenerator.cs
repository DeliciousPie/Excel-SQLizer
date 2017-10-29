using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer
{
    internal class OLD_InsertStatementGenerator
    {

        /// <summary>
        /// Gets the insert into table portion of the insert statement.
        /// e.g. INSERT INTO table_name (col1, col2, col3)
        /// </summary>
        /// <value>
        /// The insert into table.
        /// </value>
        public string InsertIntoTable { get; }

        /// <summary>
        /// Gets the insert statements.
        /// </summary>
        /// <value>
        /// The insert statements.
        /// </value>
        public List<string> InsertStatements { get; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; }

        public OLD_InsertStatementGenerator()
        {
            InsertStatements = new List<string>();
        }

        public OLD_InsertStatementGenerator(string tableName, string columns) : this()
        {
            TableName       = tableName;
            //create the InsertIntoTable statement
            InsertIntoTable = "INSERT INTO " + tableName + " (" + columns + ") ";
        }

        /// <summary>
        /// Adds an insert statement.
        /// </summary>
        /// <param name="valuesString">The values of the insert statement, comma deliminated.</param>
        public void AddInsertStatement(string valuesString)
        {
            string insert = InsertIntoTable + " VALUES (" + valuesString + ");";
            InsertStatements.Add(insert);
        }
    }
}
