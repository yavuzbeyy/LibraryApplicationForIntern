using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.SharedLibrary
{
    public static class Messages
    {
        public static string Save(string model) => $"{model} başarılı bir şekilde kayıt edildi.";
        public static string Update(string model) => $"{model} başarılı bir şekilde güncellendi.";
        public static string Delete(string model) => $"{model} başarılı bir şekilde silindi.";
        public static string NotFound(string model) => $"Böyle bir {model} kaydı bulunamamaktadır.";
        public static string SaveError(string model) => $"{model} kaydı eklenirken bir hata ile karşılaşıldı.";
        public static string DeleteError(string model) => $"{model} kaydı silinirken bir hata ile karşılaşıldı.";
        public static string UpdateError(string model) => $"{model} kaydı güncellenirken bir hata ile karşılaşıldı.";
    }
}

