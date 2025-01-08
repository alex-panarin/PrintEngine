namespace PrintEngine.Core.Models
{
	public interface IPrintEngineSettings
	{
		string QrCodeGeneratorUrl { get;  }
		string SignServiceUrl { get; }
		string MetadataServiceUrl { get; }
		string MetadataPath { get; }
		bool? UseLocalMetadata { get; }
	}
	public class PrintEngineSettings 
		: IPrintEngineSettings
	{
		public string QrCodeGeneratorUrl { get; set; }
		public string SignServiceUrl { get; set; }
		public string MetadataPath { get; set; }
		public bool? UseLocalMetadata { get; set; }
        public string MetadataServiceUrl { get; set; }
    }
}
