using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Response
{
    public class Result : IResult
    {
        public Result(bool success)
        {
            Success = success;
        }
        public Result(string message)
        {
            Message = message;
        }

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
