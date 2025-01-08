using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintEngine.Metadata.Interfaces;

namespace PrintEngine.Metadata.Services.Local
{
    public class LocalMetadataRepository : IMetadataRepository
    {
        private readonly IMetadataProvider _metadataProvider;
        private JToken? _mappings;
        private JToken? _values;

        public LocalMetadataRepository(IMetadataProvider metadataProvider)
        {
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
        }
        public async Task<IDictionary<string, string>?> LoadMapping()
        {
            var content = JsonConvert.DeserializeObject<JObject>(await _metadataProvider.LoadContentAsync())
                ?? new JObject();

            if (!content.TryGetValue("Mappings", out _mappings)
            || !content.TryGetValue("Values", out _values))
                return null;
            var mapping = ParseMapping(_mappings as JArray);
            if (mapping == null)
                return null;
            return new Dictionary<string, string>(mapping);
        }
        private IEnumerable<KeyValuePair<string, string>>? ParseMapping(JArray? mappings)
        {
            if (mappings == null)
                return null;

            return mappings
                .SelectMany(m =>
                    m.ToDictionary(k => GetKey((JProperty)k, false), v => ((JProperty)v).Value.ToString()))
                .Concat(mappings.SelectMany(m =>
                    m.ToDictionary(k => GetKey((JProperty)k, true), v => ((JProperty)v).Value.ToString())));
        }
        public Task<IDictionary<string, string>?> LoadMetadata(string metadataId)
        {
            var metadata = _values is JObject jo
                ? jo[metadataId]?.ToDictionary(k => ((JProperty)k).Name, v => ((JProperty)v).Value.ToString())
                : null;

            return Task.FromResult((IDictionary<string, string>?)metadata);
        }

        private string GetKey(JProperty jp, bool onlyName)
            => (onlyName
                ? $"{jp.Name}{MetadataService.Delimeter}{jp.Name}"
                : $"{jp.Name}{MetadataService.Delimeter}{jp.Value}")
            .ToLower();
    }
}
