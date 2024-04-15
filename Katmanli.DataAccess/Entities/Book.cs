using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public int NumberOfPages { get; set; }
        public bool isAvailable { get; set; }
        public string AuthorId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }

    }
}
