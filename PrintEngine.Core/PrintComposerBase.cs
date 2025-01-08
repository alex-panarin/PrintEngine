using Newtonsoft.Json;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;

namespace PrintEngine.Core
{
	public abstract class PrintComposerBase
		: IPrintComposer
	{
		protected IPrintMapperFactory MapperFactory { get; private set; }
		protected IPrintTemplateRepository TemplateRepository { get; }
		protected IPrintTemplateFactory TemplateFactory { get; private set; }
		public PrintComposerBase(IPrintTemplateFactory templateFactory,
			IPrintMapperFactory mapperFactory,
			IPrintTemplateRepository templateRepository)
		{
			MapperFactory = mapperFactory ?? throw new ArgumentNullException(nameof(mapperFactory));
			TemplateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
			TemplateFactory = templateFactory ?? throw new ArgumentNullException(nameof(templateFactory));
		}
		public async Task<PrintResult> Compose(ComposeRequest request, Guid? correlationId = null, bool isTestEnv = true)
		{
			try
			{
				return await ComposeEx(request, correlationId, isTestEnv);
			}
			catch (PrintTemplateException x)
			{
				return PrintResult.Fail(x.Message, correlationId);
			}
			catch (JsonException x)
			{
				return PrintResult.Fail($"{Errors.APIDOTNET_22_5} => {x.Message}", correlationId);
			}
			catch (Exception x)
			{
				return PrintResult.Fail(x.ToString(), correlationId);
			}
		}

		protected abstract Task<PrintResult> ComposeEx(ComposeRequest request, Guid? correlationId, bool? testEnvironment);
		public async Task<TemplateResult> GetTemplates()
		{
			return TemplateResult.Success(await Task.FromResult(TemplateRepository.GetTemplateNames()
				.Where(kv => kv.Key != null)
				.ToArray()));
		}
		public string GetTemplateFileName(string templateId)
		{
			return TemplateRepository.GetTemplateFileName(templateId);
		}
	}
}
