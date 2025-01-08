namespace PrintEngine.Metadata.Interfaces
{
	public interface IMetadataExplorer
	{
		Task<IDictionary<string, string>?> GetMetadataAsync(string templateId);
	}
}
