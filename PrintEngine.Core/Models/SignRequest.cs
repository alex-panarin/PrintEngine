namespace PrintEngine.Core.Models
{
	public class SignRequest
	{
		public byte[] File { get; set; }
		public string Extension { get; set;}
		public string CorrelationId { get; set;}
		public string CertKey { get; set; }
	}
}
