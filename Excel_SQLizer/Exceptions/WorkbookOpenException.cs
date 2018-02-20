using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_SQLizer.Exceptions
{
    public class WorkbookOpenException : Exception
    {
        public WorkbookOpenException() { }
        public WorkbookOpenException(string message) : base(message) { }
        public WorkbookOpenException(string message, Exception exception) : base(message, exception) { }
    }
}
