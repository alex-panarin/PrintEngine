using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using PrintEngine.Core.Interfaces;

namespace PrintEngine.Templates.Helpers
{
	internal class WatermarkProperties
	{
		public Document Document { get; set; }
		public PdfFont Font { get; set; }
		public bool PrintWatermark { get; set; }
		public string Text { get; set; } = "ОБРАЗЕЦ";
		public bool GrayColor { get; set; } = false;
		public float FontSize { get; set; } = 120f;
		public bool Condenced { get; set; } = false;
		public bool TopToRight { get; set; } = false;
	}
	internal class WatermarkHandler : IEventHandler
	{
		protected readonly WatermarkProperties _properties;

		public WatermarkHandler(WatermarkProperties properties, IResourceService<ImageData, PdfFont> resourceService)
		{
			_properties = properties ?? throw new ArgumentNullException(nameof(WatermarkProperties));
			_properties.Font = resourceService.GetArialBold();		
	
		}
		public virtual void HandleEvent(Event @event)
		{
            var page = ((PdfDocumentEvent)@event).GetPage();
            if (_properties.PrintWatermark == true)
			{
				Utils.AddWatermarkText(_properties, page);
			}
			ProcessHandle(((PdfDocumentEvent)@event).GetDocument(), page);
        }

		protected virtual void ProcessHandle(PdfDocument document, PdfPage page)
		{

		}
	}
}
