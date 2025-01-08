namespace PrintEngine.Metadata.Interfaces
{
    public interface IMetadataRepository
	{
		Task<IDictionary<string, string>?> LoadMapping();
		Task<IDictionary<string, string>?> LoadMetadata(string metadataId);
	}
}
