using Newtonsoft.Json.Linq;

namespace PrintEngine.Core.Models
{
	public class ComposeRequest
	{
		public string TemplateName { get; set; }
        public string MetadataName { get; set; }
        public JObject JsonValue { get; set; } 
		public bool? NeedSign { get; set; }
	}
}
	