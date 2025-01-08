using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
    internal class ImageParagraphRenderer : ParagraphRenderer
    {
        private readonly Image _image;

        public ImageParagraphRenderer(Paragraph modelElement, Image image)
            : base(modelElement)
        {
            _image = image;
        }
        public override void Draw(DrawContext drawContext)
        {
            base.Draw(drawContext);
            if (_image == null)
                return;

            var rect = GetOccupiedAreaBBox().ApplyMargins(-4f, 4f, 4f, 4f, false);
            var height = Math.Max(rect.GetWidth(), rect.GetHeight());
            height = Math.Min(height, 70);
            rect = new Rectangle
                (rect.GetX(),
                rect.GetY() - height / 1.7f
                , height
                , height);

            var pdfDoc = drawContext.GetDocument();
			var page = pdfDoc.GetLastPage();
			new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
				.SaveState()
                .AddXObjectFittedIntoRectangle(_image.GetXObject(), rect)
                .RestoreState();
        }

    }
}
