using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Response
{
    public class ErrorResponse<T> : Response<T>
    {
        public ErrorResponse(T data) : base(false, data)
        {
        }
        public ErrorResponse(T data, string message) : base(false, message, data)
        {
        }
        public ErrorResponse(string message) : base(false, message, default)
        {
        }
    }
}
