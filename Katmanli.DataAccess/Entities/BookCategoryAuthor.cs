﻿using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class BookCategoryAuthor : BaseEntity
    {
        public int CategoryId { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }
}
