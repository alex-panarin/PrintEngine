using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PrintEngine.Core;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using PrintEngine.Templates.Helpers;

namespace PrintEngine.Templates
{
	[Template]
	public abstract class PrintTemplateBase<TModel>
		: IPrintTemplate
		where TModel : class, IPrintModel
	{
		public async Task<(PrintData, SignData)> GetDocument(IPrintModel model, bool needSign, bool printDemonstration = false, string correlationId = null)
		{
			using var stream = new MemoryStream();
			var document = new PdfDocument(new PdfWriter(stream, new WriterProperties()
				.SetCompressionLevel(CompressionConstants.BEST_SPEED)
				.SetFullCompressionMode(true)
				.UseSmartMode()
				));
			using var layout = new Document(document);

			Model = (TModel)model;

			var startEventHandler = GetStartPageEventHandler(layout, printDemonstration);
			if (startEventHandler != null)
				document.AddEventHandler(PdfDocumentEvent.START_PAGE, startEventHandler);

			var endEventHandler = GetEndPageEventHandler(layout);
			if (endEventHandler != null)
				document.AddEventHandler(PdfDocumentEvent.END_PAGE, endEventHandler);

			await ProcessTemplate(layout);
			// Call before layout Closed
			var pageCount = document.GetNumberOfPages();
			layout.Close();// Calls document close also
			var outData = stream.ToArray();
			var templateFileName = GetTemplateFileName();
			var fileName = $"{templateFileName}.{(needSign ? "pdf" : "out")}";
			var printData = new PrintData
			{
				FileOutput = outData,
				OutputLength = Convert.ToUInt32(outData.Length),
				PageCount = pageCount,
				FileName = fileName,
				FileType = "pdf",
			};

			SignData signData = null;
			if (needSign)
			{
				if (IsNeedSignDocument == false)
					throw new PrintTemplateException($"Печатная форма {TemplateId} не требует подписания.");

				signData = new SignData
				{
					FileOutput = await SignService?.SignDocument(new SignRequest
					{
						Extension = "pdf",
						File = outData,
						CorrelationId = correlationId,
						CertKey = templateFileName
					}),

					FileName = $"{templateFileName}.pdf.sgn"
				};

				// Внедрение сертификата в документ
				using var inStream = new MemoryStream(printData.FileOutput);
				using var outStream = new MemoryStream();
				using var tmpDoc = new PdfDocument(new PdfReader(inStream), new PdfWriter(outStream));

				SignDocument(tmpDoc, signData.FileOutput);

				tmpDoc.Close();
				printData.FileOutput = outStream.ToArray();

			}
			return (printData, signData);
		}

		protected virtual IEventHandler GetStartPageEventHandler(Document layout, bool printDemonstration)
		{
			return new PdfNewPageEventHanler(printDemonstration);
		}
		protected virtual IEventHandler GetEndPageEventHandler(Document layout)
		{
			return null;
		}

		// Точка входа для создания документа
		protected abstract Task ProcessTemplate(Document layout);

		protected virtual void SignDocument(PdfDocument document, byte[] certificate)
		{

		}
		protected abstract string GetTemplateFileName();
		public Type GetModelType()
		{
			return typeof(TModel);
		}
		public abstract Type[] GetValidInputDataTypes();
		void IPrintTemplate.SetContext(IPrintEngineContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			ResourceService = (IResourceService<ImageData, PdfFont>)context.ResourceService;
		}
		void IPrintTemplate.SetTemplateId(string templateId)
		{
			TemplateId = templateId;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				_qrCodeService?.Dispose();
				_signService?.Dispose();
				_qrCodeService = null;
				_signService = null;
				ResourceService = null;
			}
		}
		protected TModel Model { get; private set; }

		private IPrintEngineContext _context;
		private IQrCodeService<Image> _qrCodeService;
		private ISignService _signService;

		protected ISignService SignService => _signService ??= _context?.SignService;
		protected IQrCodeService<Image> QrCodeService => _qrCodeService ??= (IQrCodeService<Image>)_context?.QrCodeService;
		protected IResourceService<ImageData, PdfFont> ResourceService { get; private set; }
		protected string TemplateId { get; private set; }
		protected virtual bool IsNeedSignDocument => false;
	}
}
