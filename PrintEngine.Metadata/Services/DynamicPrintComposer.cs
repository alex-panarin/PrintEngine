using PrintEngine.Core;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using PrintEngine.Metadata.Interfaces;

namespace PrintEngine.Metadata.Services
{
	public class DynamicPrintComposer
		: PrintComposerBase
		, IMetadataExplorer
	{
		private readonly IMetadataService _metadataService;

		public DynamicPrintComposer(IPrintTemplateFactory templateFactory
			, IPrintMapperFactory mapperFactory
			, IMetadataService metadataService
			, IPrintTemplateRepository templateRepository)
			: base(templateFactory
				  , mapperFactory
				  , templateRepository)
		{
			_metadataService = metadataService ?? throw new ArgumentNullException(nameof(metadataService));
		}

		protected override async Task<PrintResult> ComposeEx(ComposeRequest request, Guid? correlationId, bool? testEnvironment)
		{
			var input = request?.JsonValue;
			if (input == null)
				throw new PrintTemplateException(Errors.APIDOTNET_22_4);

			if (string.IsNullOrEmpty(request?.TemplateName))
				throw new PrintTemplateException(Errors.APIDOTNET_22_3);

			using var template = TemplateFactory.GetTemplate(request.TemplateName);
			if (template == null)
				throw new PrintTemplateException($"Шаблон {request.TemplateName} не зарегистрирован");

			var metadata = await _metadataService.GetMetadata(request.TemplateName, request.MetadataName);
			if (metadata == null)
				throw new PrintTemplateException(Errors.APIDOTNET_22_1);

			var mapper = MapperFactory.GetMapper(input.GetType(), template.GetModelType());
			if (mapper == null)
				throw new PrintTemplateException(Errors.APIDOTNET_22_6);

			var model = mapper.Map(input, metadata);
			if (model == null)
				throw new PrintTemplateException(Errors.APIDOTNET_22_2);

			model.Validate();

			var (printData, signData) = await template.GetDocument(model
				, request.NeedSign == true
				, testEnvironment == true
				, correlationId?.ToString());

			return PrintResult.Success(printData, signData, correlationId);
		}

		public async Task<IDictionary<string, string>?> GetMetadataAsync(string templateName)
		{
			return await _metadataService.GetMetadata(templateName);
		}
	}
}
