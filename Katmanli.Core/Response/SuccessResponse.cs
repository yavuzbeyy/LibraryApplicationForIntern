using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Response
{
    public class SuccessResponse<T> : Response<T>
    {
        public SuccessResponse(T data) : base(true, data)
        {
        }

        public SuccessResponse(T data, string message) : base(true, message, data)
        {
        }

        public SuccessResponse(string message) : base(true, message, default)
        {
        }
    }
}
