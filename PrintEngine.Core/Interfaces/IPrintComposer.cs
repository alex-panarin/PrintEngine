using PrintEngine.Core.Models;

namespace PrintEngine.Core.Interfaces
{
	public interface IPrintComposer
	{
		Task<PrintResult> Compose(ComposeRequest request, Guid? correlationId = null, bool isTestEnv = true);
		Task<TemplateResult> GetTemplates();
		string GetTemplateFileName(string templateId);
	}
}
