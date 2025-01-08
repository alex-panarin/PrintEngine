using iText.Barcodes.Qrcode;
using iText.Barcodes;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using PrintEngine.Core.Interfaces;
using PrintEngine.Templates.Helpers;
using PrintEngine.Core.Services;
using System;
using iText.Commons.Actions.Contexts;

namespace PrintEngine.Templates.Services
{
    public class QrCodeService
        : BaseExternalService<QrCodeContext, Image>
        , IQrCodeService<Image>
    {
        const string dkbm = "https://dkbm-web.autoins.ru/dkbm-web-1.0/qr.htm?id=";
        public QrCodeService(IHttpClientFactory factory, string url)
            : base(factory)
        {
            QrCodeUrl = url;
        }
        protected string QrCodeUrl { get; }
        public async Task<Image> GetQrCode(QrCodeContext context)
        {
            if (context.Document != null)
            {
                var hints = new Dictionary<EncodeHintType, object>(
                    new[]
                    {
                        new KeyValuePair<EncodeHintType, object>(EncodeHintType.CHARACTER_SET, "UTF-8")
                    }
                );
                BarcodeQRCode qrCode = new BarcodeQRCode(GetUrl(context), hints);
                var pdfFormXObject = qrCode.CreateFormXObject(ColorConstants.BLACK, (PdfDocument)context.Document);
                return new Image(pdfFormXObject);
            }

            return await SendRequestAsync(context);
        }

        protected override async Task<Image> SendRequest(HttpClient httpClient, QrCodeContext request)
        {
            var bytes = await httpClient.GetByteArrayAsync(GetUrl(request));
            return new Image(ImageDataFactory.Create(bytes));
        }

        private string GetUrl(QrCodeContext context)
        {
            if (context.Document != null)
            {
                var content = context.ContentUrl.IsEmpty()
                    ? context.Content?.Split('=').LastOrDefault()
                    : context.ContentUrl.Split('/').LastOrDefault();
                return $"{dkbm}{content}";
            }
            var url = QrCodeUrl.EndsWith("/") ? QrCodeUrl : $"{QrCodeUrl}/";
            return context.ContentUrl ?? $"{url}1/{context.Content?.Split('=').LastOrDefault()}";
        }
    }
}
