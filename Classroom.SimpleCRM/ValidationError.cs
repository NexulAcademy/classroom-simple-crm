using System;
using System.Collections.Generic;
using System.Text;

namespace Classroom.SimpleCRM
{
    public class ValidationError
    {
        public string RecordId { get; set; }
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
