namespace PrintEngine.Core.Interfaces
{
	public interface IPrintMapper
    {
		IPrintModel Map(object value, IDictionary<string, string> properties = null);
	}
}
