using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using PrintEngine.Core.Interfaces;
using PrintEngine.Resources.Repositories;

namespace PrintEngine.Templates.Services
{
    public class ResourceService
        : IResourceService<ImageData, PdfFont>
    {
        private readonly IResourceRepository _repository;

        public ResourceService(IResourceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public PdfFont GetFont(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return PdfFontFactory.CreateFont(_repository.GetFontBytes(key), PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
        }

        public ImageData GetImage(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return ImageDataFactory.Create(_repository.GetImageBytes(key));
        }
    }
}
