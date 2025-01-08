namespace PrintEngine.Metadata.Interfaces
{
	public interface IMetadataService
	{
		Task<IDictionary<string, string>?> GetMetadata(string templateName, string? metadataName = null);
	}
}
