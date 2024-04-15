using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Response
{
    public interface IResponse<T> : IResult
    {
        T Data { get; set; }
    }
}
