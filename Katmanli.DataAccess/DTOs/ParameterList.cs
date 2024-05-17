using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

    public class ParameterList : IEnumerable<Parameter>
        {
            public List<Parameter> Parameters { get; set; }

            public ParameterList()
            {
                Parameters = new List<Parameter>();
            }

            public void Add(string name, object value)
            {
                Parameters.Add(new Parameter() { Name = name, Value = value });
            }

            public IEnumerator<Parameter> GetEnumerator()
            {
                return Parameters.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Reset()
             {
             Parameters = new List<Parameter>();
             }
    }
    }

