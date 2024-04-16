using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class CategoryDTO
    {
        public class CategoryCreate
        {
            public string Name { get; set; }
        }

        public class CategoryDelete
        {

        }

        public class CategoryUpdate
        {

        }

        public class CategoryQuery
        {

            public int Id { get; set; }
            public string Name { get; set; }

        }
    }
}
