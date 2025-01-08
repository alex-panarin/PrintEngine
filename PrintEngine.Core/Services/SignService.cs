using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using System.Net.Http.Json;
using System.Text;

namespace PrintEngine.Core.Services
{
	public class SignService
		: BaseExternalService<SignRequest, byte[]>
		, ISignService
	{
		private readonly string _signServiceUrl;

		public SignService(IHttpClientFactory factory, string url)
			: base(factory)
		{
			_signServiceUrl = $"{url}/SignByteArray";
		}

		public async Task<byte[]> SignDocument(SignRequest request)
		{
			return await SendRequestAsync(request);
		}

		protected override async Task<byte[]> SendRequest(HttpClient httpClient, SignRequest request)
		{
			return await SendRequestStub();
			var content = JsonContent.Create(request);
			var answer = await httpClient.PostAsync(_signServiceUrl, content);
			answer.EnsureSuccessStatusCode();

			var result = await answer.Content.ReadFromJsonAsync<SignResult>();
			return Convert.FromBase64String(result.certFile);
		}

		private Task<byte[]> SendRequestStub()
		{
			return Task.FromResult(Encoding.UTF8.GetBytes("MIAGCSqGSIb3DQEHAqCAMIAC..."));
		}

		class SignResult
		{
			public string certFile { get; set; }
			public string correlationId { get; set; }
		}
	}
}
