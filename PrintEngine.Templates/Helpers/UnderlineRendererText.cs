using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
    internal class UnderlineRendererText : UnderlineCellRenderer
	{
		private readonly PdfFont _font;
        private readonly float _yOffset;
        private readonly float _fontSize;
		private readonly TextAlignment _alignment;
        private readonly Paragraph[] _paragraphs;

        public UnderlineRendererText(Cell modelElement
			, string text
			, PdfFont font
			, float yOffset = 0f
			, float width = 0.5f
			, float fontSize = 6f
			, TextAlignment alignment = TextAlignment.CENTER) 
			: this(modelElement
				  , text?.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)?.Select(t => new Paragraph(t)).ToArray()
				  , font
				  , yOffset
				  , width
				  , fontSize
				  , alignment)
		{
			
		}
        public UnderlineRendererText(Cell modelElement
            , Paragraph[] paragraphs
            , PdfFont font
            , float yOffset = 0f
            , float width = 0.5f
            , float fontSize = 6f
            , TextAlignment alignment = TextAlignment.CENTER)
            : base(modelElement, false, width)
        {
            _paragraphs = paragraphs;
            _font = font;
            _yOffset = yOffset;
            _fontSize = fontSize;
            _alignment = alignment;
        }

        public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);

			var canvas = drawContext
				.GetCanvas()
				.SaveState();
				
			DrawText(canvas);
			canvas.RestoreState();

		}
		protected void DrawText(PdfCanvas canvas)
		{
			var rect = GetOccupiedAreaBBox().ApplyMargins(0, 0, 2f, 0, false);
			canvas
				.SetLineWidth(_width)
				.MoveTo(rect.GetLeft(), rect.GetBottom())
				.LineTo(rect.GetRight(), rect.GetBottom())
				.Stroke();
			//
			if (_paragraphs.IsEmpty() == true)
				return;
			
			/// devide by 1000 - cause is a 1000-based return value of function GetContentWidth,
			/// and font size
			var yOffset = 0;
			var newRect = new Rectangle(rect).MoveDown(_fontSize * 1.8f + _yOffset);

            foreach (var para in _paragraphs)
			{
				using var layout = new Canvas(canvas, newRect.MoveDown(_fontSize * yOffset ++));
				layout.SetFont(_font).SetFontSize(_fontSize);
				layout.Add(para.SetTextAlignment(_alignment));
			}
		}
	}
}
