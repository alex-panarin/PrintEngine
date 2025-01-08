using System.ComponentModel;

namespace PrintEngine.Core.Models
{
	public class SignData
	{
        [Description("Имя файла ЭЦП")]
        public string FileName { get; set; }
        
        [Description("Контент файла ЭЦП")]
        public byte[] FileOutput { get; set; }
   }
}
