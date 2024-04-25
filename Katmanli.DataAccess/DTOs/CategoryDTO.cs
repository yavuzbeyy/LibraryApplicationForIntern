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
            public string? Name { get; set; }

            public string? Description { get; set; }
        }

        public class CategoryDelete
        {
            public int Id { get; set; }
        }

        public class CategoryUpdate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class CategoryQuery
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }

        }
    }
}
