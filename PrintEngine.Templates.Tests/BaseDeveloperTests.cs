using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PrintEngine.Core.Models;
using PrintEngine.Templates.Infrastructure;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrintEngine.Templates.Tests
{
	public abstract class BaseDeveloperTests
	{
		static IServiceCollection _services = new ServiceCollection();
		protected static ServiceProvider _serviceProvider;
		protected static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			Formatting = Newtonsoft.Json.Formatting.Indented,
			DateParseHandling = DateParseHandling.None,
		};
		protected static PrintEngineSettings _printSettings = new PrintEngineSettings
		{
			QrCodeGeneratorUrl = "",
			SignServiceUrl = "",
			UseLocalMetadata = true,
			MetadataPath = "MappingMetadata",
		};

		[OneTimeSetUp]
		public void Setup()
		{
			_serviceProvider = _services
				.RegisterPrintEngine(_printSettings)
				.BuildServiceProvider();
		}
		[OneTimeTearDown]
		public void TearDown() 
		{ 
			_serviceProvider?.Dispose();
			_serviceProvider = null;
		}
		protected void SavePdfToFile(PrintData data, [CallerMemberName] string filename = null)
		{
			if (data == null) return;

			// TODO: доделать настройки на среду разработки
			if (UseDevelopmentTool)
			{
				Task.Factory.StartNew(async () =>
				{
					try
					{
						const int contentLength = ushort.MaxValue;
						
						using (var client = new TcpClient())
						{
							await client.ConnectAsync(IPAddress.Loopback, 9996);

							using var stream = new MemoryStream() ;

                            // write file information
                            var info = new PdfFileContext
                            {
                                FileName = data.FileName,
                                Length = (int)data.OutputLength,
                                Model = GetModel()
                            };
                            var infoBytes = info.ToByteArray();
							await stream.WriteAsync(infoBytes);
                            // write file content
                            await stream.WriteAsync(data.FileOutput);
							
							var socket = client.GetStream();
							// send file content
							stream.Position = 0;
							await stream.CopyToAsync(socket, contentLength);
						
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"==== Error: {ex} =====");
					}
				});

				return;
			}

			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{DocumentFolderName}");
			Directory.CreateDirectory(path);
			
			path = Path.Combine(path, $"{filename}.pdf");

			File.WriteAllBytes(path, data.FileOutput);
		}

		protected virtual string DocumentFolderName { get; set; } = "Test";
		protected bool UseDevelopmentTool { get; set; } = false;
		protected abstract string GetModel();

		class PdfFileContext
		{
            const short fileCtxSize = ushort.MaxValue / 2;
            public int Length { get; set; }
			public string FileName { get; set; }
            public string Model { get; set; }
            public byte[] ToByteArray()
			{
				var jsonCtx = JsonConvert.SerializeObject(this).PadRight(fileCtxSize);
                return Encoding.UTF8.GetBytes(jsonCtx);
			}
		}
	}
}
