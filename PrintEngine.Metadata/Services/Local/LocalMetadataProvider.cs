using PrintEngine.Core.Models;
using PrintEngine.Resources;

namespace PrintEngine.Metadata.Services.Local
{
    public interface IMetadataProvider
    {
        Task<string> LoadContentAsync();
    }
    public class LocalMetadataProvider : IMetadataProvider
    {
        private readonly IPrintEngineSettings _engineSettings;

        public LocalMetadataProvider(IPrintEngineSettings engineSettings)
        {
            _engineSettings = engineSettings ?? throw new ArgumentNullException(nameof(engineSettings));
        }
        public async Task<string> LoadContentAsync()
        {
            return await Task.FromResult(ResourceHelper.GetMetadata(_engineSettings.MetadataPath));
            //return await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, _engineSettings.MetadataPath));
        }
    }
}
