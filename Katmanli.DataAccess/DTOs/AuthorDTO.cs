using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{

        public class AuthorCreate
        {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? YearOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }

        public string? fotoKey { get; set; }
    }


        public class AuthorDelete
        {
            public int Id { get; set; }
        }

        public class AuthorUpdate
        {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? YearOfBirth { get; set; }

        public string? PlaceOfBirth { get; set; }
    }

        public class AuthorQuery
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int YearOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? fotoKey { get; set; }
    }
    }

