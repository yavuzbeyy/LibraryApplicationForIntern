using Katmanli.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Entities
{
    public class UploadImage : BaseEntity
    {
        public string? FileOriginalName { get; set; }
        public string? FileGuidedName { get; set; }
        public string? FileKey { get; set; }
        public string? FilePath { get; set; }
        //public int? BookId { get; set; }
    }
}
