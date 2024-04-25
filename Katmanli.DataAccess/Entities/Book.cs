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
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? PublicationYear { get; set; }
        public int? NumberOfPages { get; set; }
        public bool? isAvailable { get; set; }
        public string? FileKey { get; set; }
    }
}
