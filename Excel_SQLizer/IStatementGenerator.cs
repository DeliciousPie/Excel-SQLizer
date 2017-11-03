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
        /// <param name="values">The values to add. List of objects (strings or numbers).</param>
        void AddStatement(List<object> values);


        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <returns>A list of all generated statements</returns>
        List<string> GetStatements();
    }
}
