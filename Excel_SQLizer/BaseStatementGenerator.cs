﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer
{
    public abstract class BaseStatementGenerator
    {

        /// <summary>
        /// The statements generated by the StatementGenerator
        /// </summary>
        protected List<string> _statements;
        /// <summary>
        /// The file name of the generated statements.
        /// </summary>
        protected string _fileName;
        /// <summary>
        /// The columns of the table.
        /// </summary>
        protected string _columns;
        /// <summary>
        /// The table name
        /// </summary>
        protected string _tableName;

        /// <summary>
        /// The primary key column.
        /// </summary>
        protected string _idCol;

        /// <summary>
        /// An array of the column names
        /// </summary>
        protected string[] _colArray;

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <value>
        /// The statements.
        /// </value>
        public List<string> Statements
        {
            get { return _statements; }
        }

        /// <summary>
        /// Initializes generator properties.
        /// </summary>
        /// <param name="statementPrefix">The statement prefix.</param>
        /// <param name="whereClause">The where clause.</param>
        protected void Initialize(string columns, string fileName, string tableName)
        {
            _columns    = columns;
            _colArray   = columns.Split(',');
            _fileName   = fileName;
            _tableName  = tableName;
            _statements = new List<string>();
        }

        /// <summary>
        /// Adds the statement to the list of generated statements.
        /// </summary>
        /// <param name="values">The values to add. A list of objects (strings or numbers)</param>
        public abstract void AddStatement(List<object> values);

    }
}
