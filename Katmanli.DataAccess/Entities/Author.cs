using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string YearOfBirth { get; set; }
        public List<Book> Books { get; set; }
    }
}
