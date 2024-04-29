using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class UserMessages : BaseEntity
    {
        public string? Message { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public bool? isAdminMessage { get; set; }
    }
}
