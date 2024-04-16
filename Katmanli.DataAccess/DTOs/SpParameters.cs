using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class SpParameters 
    {
        public int? FindById { get; set; }

        public int? DeleteById { get; set; }

        public string? FindByName { get; set; }

        public SpParameters()
        {
            FindById = null;
            DeleteById = null;
            FindByName = null;
        }

        public void Reset()
        {
            FindById = null;
            DeleteById = null;
            FindByName = null;
        }
    }
}
