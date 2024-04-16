using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    using System.Collections;
    using System.Collections.Generic;


        public class ParameterList : IEnumerable<Parameter>
        {
            public List<Parameter> Parameters { get; set; }

            public ParameterList()
            {
                Parameters = new List<Parameter>();
            }

            public void Add(Parameter parameter)
            {
                Parameters.Add(parameter);
            }

            // Implementation of IEnumerable<Parameter>
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

