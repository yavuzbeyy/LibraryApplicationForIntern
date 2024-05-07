using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class UploadImagesDTO
    {
        public int? Id { get; set; }
        public string? FileKey { get; set; }
        public string? FilePath { get; set; }
    }
}
