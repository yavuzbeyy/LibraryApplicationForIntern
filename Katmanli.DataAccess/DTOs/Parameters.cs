using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class Parameter
    {
        public string? Name { get; set; }
        public object? Value { get; set; }

        public void addParameter(string parameterName,object value) 
        { 
           Name = parameterName;
           Value = value;
        }
    }
}
