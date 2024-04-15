using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Response
{
    public interface IResult
    {
        public bool Success { get; }
        public string Message { get; }
    }
}
