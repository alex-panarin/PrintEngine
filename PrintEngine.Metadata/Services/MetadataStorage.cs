using PrintEngine.Core;

namespace PrintEngine.Metadata.Services
{
    internal class MetadataStorage
    {
        private Dictionary<string, string>? _metadataMapping = null;
        private Dictionary<string, Dictionary<string, string>> _metadataValues = new();
        private static MetadataStorage? _metadataStorage = null;
        internal static MetadataStorage Instance => _metadataStorage ??= new MetadataStorage();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        internal async Task<IDictionary<string, string>> GetOrAddMetadata(
            string mappingKey
            , Func<Task<IDictionary<string, string>?>> mappingFunc
            , Func<string, Task<IDictionary<string, string>?>> metadataFunc)
        {
            try
            {
                _semaphore.Wait();

                if (_metadataMapping == null)
                {
                    var mapping = await mappingFunc();
                    if (mapping == null)
                        throw new Exception(Errors.APIDOTNET_22_7);

                    _metadataMapping = new(mapping);
                }
                if (_metadataMapping.TryGetValue(mappingKey, out var key) == false)
                    throw new Exception(Errors.APIDOTNET_22_1);

                if (_metadataValues.ContainsKey(key))
                    return _metadataValues[key];

                var values = await metadataFunc(key);
                if (values == null)
                    throw new Exception(Errors.APIDOTNET_22_1);

                return _metadataValues[key] = new(values);
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
