namespace PrintEngine.Core.Interfaces
{
	public interface IPrintTemplateFactory
    {
        IPrintTemplate GetTemplate(string Id);
	}
}
