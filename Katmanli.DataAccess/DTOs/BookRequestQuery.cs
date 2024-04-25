using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class BookRequestQuery
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? isApproved { get; set; }
       // public string? BookName { get; set; }
        //public string? Username { get; set;}
    }

    public class BookRequestCreateDto 
    {
        public DateTime? CreatedDate { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? isApproved { get; set; }
    }
}
