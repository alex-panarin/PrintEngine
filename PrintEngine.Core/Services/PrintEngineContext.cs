using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;

namespace PrintEngine.Core.Services
{
	public class PrintEngineContext
		: IPrintEngineContext
	{
		private readonly Func<IHttpClientFactory, string, ISignService> _signFactory;
		private readonly Func<IHttpClientFactory, string, IQrCodeService> _qrCodeFactory;
		private readonly IPrintEngineSettings _settings;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IResourceService _resourceService;

		public PrintEngineContext(
			Func<IHttpClientFactory, string, ISignService> signFactory,
			Func<IHttpClientFactory, string, IQrCodeService> qrCodeFactory,
			IPrintEngineSettings settings,
			IHttpClientFactory httpClientFactory,
			IResourceService resourceService)
		{
			_signFactory = signFactory ?? throw new ArgumentNullException(nameof(signFactory));
			_qrCodeFactory = qrCodeFactory ?? throw new ArgumentNullException(nameof(qrCodeFactory));
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
		}
		public ISignService SignService => _signFactory(_httpClientFactory, _settings.SignServiceUrl);
		public IQrCodeService QrCodeService => _qrCodeFactory(_httpClientFactory, _settings.QrCodeGeneratorUrl);
		public IResourceService ResourceService => _resourceService;
		public IPrintEngineSettings Settings => _settings;
	}
}
