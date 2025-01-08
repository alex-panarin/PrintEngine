namespace PrintEngine.Core.Interfaces
{
	public interface IPrintTemplateRepository
	{
		string GetTemplateFileName(string templateId);
		KeyValuePair<string, string>[] GetTemplateNames();
		void Register(KeyValuePair<string, string>[] values);
	}
}
