using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer
{
    public interface ISQLizer
    {
        Dictionary<string, List<string>> GetSQLStatements();
    }
}
