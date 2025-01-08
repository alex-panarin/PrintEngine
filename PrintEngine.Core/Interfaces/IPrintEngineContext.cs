using PrintEngine.Core.Models;

namespace PrintEngine.Core.Interfaces
{
	public interface IPrintEngineContext
	{
		ISignService SignService { get; }
		IQrCodeService QrCodeService { get; }
		IResourceService ResourceService { get; }
		IPrintEngineSettings Settings { get;}
	}
}
