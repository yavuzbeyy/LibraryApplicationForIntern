using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.BaseEntity
{
    //Base yapının nesnesi oluşturulmasın diye sınıfı soyut yaptım.
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
