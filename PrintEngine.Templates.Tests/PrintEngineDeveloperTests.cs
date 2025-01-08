using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using PrintEngine.Metadata.Interfaces;
using PrintEngine.Templates.Infrastructure;
using PrintEngine.Templates.UserModels;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;

namespace PrintEngine.Templates.Tests
{
    [Explicit]
	public class PrintEngineDeveloperTests
		: BaseDeveloperTests
	{
		static IServiceCollection _services = new ServiceCollection();
		
		static string sampleTemplateId = "SAMPLE";
		

		[SetUp]
		public void Setup()
		{
			_serviceProvider = _services
				.AddSingleton(_printSettings)
				.RegisterPrintEngine(_printSettings)
				.BuildServiceProvider();
		}
		/// <summary>
		/// Для печати документа на основе Json для входных данных
		/// </summary>
		[Test]
		[Explicit]
		public async Task MakePdfFromJsonString()
		{
			var composer = _serviceProvider.GetService<IPrintComposer>();
			Assert.That(composer, Is.Not.Null);
			var request = new ComposeRequest
			{
				MetadataName = sampleTemplateId,
				TemplateName = sampleTemplateId,
				JsonValue = JsonConvert.DeserializeObject<JObject>("")
			};
			// Если указать правильные входные данные:
			var result = await composer.Compose(request); //JsonObject.Parse(Resource.InputDataJson).AsObject());
			Assert.That(result.PrintData, Is.Not.Null);
			SavePdfToFile(result.PrintData, nameof(MakePdfFromJsonString));

		}

		[Test]
		[Explicit]
		public async Task CheckTemplateRegistration_Valid()
		{
			var composer = _serviceProvider.GetService<IPrintComposer>();
			Assert.That(composer, Is.Not.Null);

			var result = await composer.GetTemplates();
			Assert.That(result, Is.Not.Null);
			Assert.Pass(JsonConvert.SerializeObject(result.Templates));
		}

		[Test]
		[Explicit]
		public async Task CantMakePdfFromNotFoundTemplate()
		{
			var composer = _serviceProvider.GetService<IPrintComposer>();
			Assert.That(composer , Is.Not.Null);
			var request = new ComposeRequest
			{
				MetadataName = string.Empty,
				TemplateName = string.Empty,
				JsonValue = JsonConvert.DeserializeObject<JObject>("{\"Some\":\"JsonString\"}")
			};
			// В данном случае не зарегестрирован шаблон для пачати с именем "templatename"
			var errorResult = await composer.Compose(request);
			Assert.That(errorResult.IsSuccess, Is.False);
			Assert.That(!string.IsNullOrEmpty(errorResult.Error), Is.True);
			Assert.That(errorResult.PrintData == null, Is.True);
		}
		/// <summary>
		/// Для быстрого тестирования шаблона на основе Json
		/// </summary>
		[Test]
		[Explicit]
		public async Task MakePdfFromJsonPrintModel()
		{
			var factory = _serviceProvider.GetService<IPrintTemplateFactory>();
			var template = factory.GetTemplate(sampleTemplateId);
			Assert.That(template, Is.Not.Null);

			var model = System.Text.Json.JsonSerializer.Deserialize<PrintModelSample>("",
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
			Assert.That(model, Is.Not.Null);

			var data = await template.GetDocument(model, false);
			//Assert.That(data, Is.Not.Null);

			SavePdfToFile(data.Item1, nameof(MakePdfFromJsonPrintModel));
		}

		
		
		
				
		
		protected override string GetModel()
		{
			throw new NotImplementedException();
		}
	}
	class TestDataModel
	{
		public string TemplateId;
		public string ModelName;
	}
}