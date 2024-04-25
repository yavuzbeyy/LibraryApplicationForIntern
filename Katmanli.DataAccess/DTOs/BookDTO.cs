using Katmanli.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{

    public class BookCreate
    {
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public int NumberOfPages { get; set; }
        public bool isAvailable { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string? Filekey { get; set; }

        public string? Description { get; set; }
    }

    public class BookDelete
    {

    }

    public class BookUpdate
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? PublicationYear { get; set; }
        public int? NumberOfPages { get; set; }
        public bool? isAvailable { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
    }

    public class BookQuery
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public int NumberOfPages { get; set; }
        public bool isAvailable { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorSurname { get; set; }
        public string? CategoryName { get; set; }
        public string? Filekey { get; set; }
        public string? Description { get; set; }
    }

}
