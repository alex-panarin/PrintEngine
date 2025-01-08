using PrintEngine.Metadata.Interfaces;

namespace PrintEngine.Metadata.Services
{
    public class MetadataService : IMetadataService
    {
        public const string Delimeter = ":";
        private readonly IMetadataRepository _metadataRepository;

        public MetadataService(IMetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
        }
        public async Task<IDictionary<string, string>?> GetMetadata(string templateName, string? metadataName = null)
        {
            return await MetadataStorage.Instance.GetOrAddMetadata(
                GetMappingKey(templateName, metadataName)
                , _metadataRepository.LoadMapping
                , _metadataRepository.LoadMetadata);
        }
        private string GetMappingKey(string templateId, string? metadataId) =>
            (string.IsNullOrWhiteSpace(metadataId) || metadataId == templateId
                ? $"{templateId}{Delimeter}{templateId}"
                : $"{templateId}{Delimeter}{metadataId}")
            .ToLower();
    }
}
