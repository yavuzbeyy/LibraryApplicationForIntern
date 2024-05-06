using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class UserInGroups : BaseEntity
    {
        public int? UserMessageGroupsId { get; set; }
        public int? UserId { get; set; }
    }
}
