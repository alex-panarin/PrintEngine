namespace PrintEngine.Core.Interfaces
{
    public class QrCodeContext
	{
        public string Content { get; set; }
        public string ContentUrl { get; set; }
		public object Document { get; set; }
		public bool UsePrefix { get; set; }
    }
    public interface IQrCodeService
        : IDisposable
    {
    }
	public interface IQrCodeService<TQrCode>
		: IQrCodeService
        where TQrCode : class
	{
		Task<TQrCode> GetQrCode(QrCodeContext qrCodeContext);
	}
}
