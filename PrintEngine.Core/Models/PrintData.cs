using System.ComponentModel;

namespace PrintEngine.Core.Models
{
    public class PrintData
    {
        [Description("Количество страниц")]
        public int PageCount { get; set; }

        [Description("Имя файла")]
        public string FileName { get; set; }

        [Description("Контент файла")]
        public byte[] FileOutput { get; set; }

        [Description("Размер файла")]
        public uint OutputLength { get; set; }

        [Description("Тип файла")]
        public string FileType { get; set; }
    }
}
