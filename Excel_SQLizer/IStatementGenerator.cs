using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer
{
    internal interface IStatementGenerator
    {

        /// <summary>
        /// Adds the statement to the list of generated statements.
        /// </summary>
        /// <param name="values">The values to add. Comma delimited.</param>
        void AddStatement(string values);


        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <returns>A list of all generated statements</returns>
        List<string> GetStatements();
    }
}
