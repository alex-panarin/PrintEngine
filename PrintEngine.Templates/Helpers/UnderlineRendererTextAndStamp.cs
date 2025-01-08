using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Renderer;


namespace PrintEngine.Templates.Helpers
{
	internal class UnderlineRendererTextAndStamp : UnderlineRendererText
	{
		private readonly Image _stamp;

		public UnderlineRendererTextAndStamp(Cell modelElement, Image stamp, string text, PdfFont font, float width = 0.5F) 
			: base(modelElement, text, font, width)
		{
			_stamp = stamp;
		}

		public override void Draw(DrawContext drawContext)
		{
			var canvas = drawContext
				.GetCanvas()
				.SaveState();

			DrawText(canvas);
			canvas.RestoreState();
			
			if (_stamp != null)
			{
				var pdfDoc = drawContext.GetDocument();
				var page = pdfDoc.GetLastPage();
				var rect = GetOccupiedAreaBBox();
				var centerX = rect.GetLeft() + rect.GetWidth() / 2;
				var canterY = rect.GetBottom() + rect.GetHeight() / 2;
				var imageRect = new Rectangle(centerX - 65, canterY - 55, SignatureAndStampRenderer.StampWidth, SignatureAndStampRenderer.StampHeight);
				canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
					.SaveState()
					.AddXObjectFittedIntoRectangle(_stamp.GetXObject(), imageRect)
					.RestoreState();
			}
			
		}
	}
}
