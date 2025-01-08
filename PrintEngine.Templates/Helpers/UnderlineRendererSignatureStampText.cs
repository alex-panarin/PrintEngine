using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class UnderlineRendererSignatureStampText : CellRenderer
	{
		private readonly Image _signature;
		private readonly Image _stamp;
		private readonly string _text;
		private readonly PdfFont _font;
		private readonly bool _applyPadding;
		private readonly float _moveUpSign;
		private readonly float _moveUpStamp;
		private readonly float _fontSize = 6f;
		private readonly float _width = 0.5f;

		public UnderlineRendererSignatureStampText(Cell modelElement, Image signature, Image stamp, string text, PdfFont font, bool applyPadding, float moveUpSign = 20f, float moveUpStamp = 10f)
			: base(modelElement)
		{
			_signature = signature;
			_stamp = stamp;
			_text = text;
			_font = font;
			_applyPadding = applyPadding;
			_moveUpSign = moveUpSign;
			_moveUpStamp = moveUpStamp;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);

			var rect = GetOccupiedAreaBBox();
			var underRect = rect.Clone().ApplyMargins(0, 0, 2f, 0, false);
			var canvas = 
				drawContext.GetCanvas()
				.SaveState()
				.SetLineWidth(_width)
				.MoveTo(underRect.GetLeft(), underRect.GetBottom());
			canvas.LineTo(underRect.GetRight(), underRect.GetBottom());
			canvas.Stroke();
		
			//var _font = Utils.GetArial();
			/// devide by 1000 - cause is a 1000-based return value of function GetContentWidth,
			/// and font size
			var width = _font.GetContentWidth(new PdfString(_text, Utils.CyrEncoding)) / 1000 * _fontSize;
			var x = (underRect.GetWidth() / 2 - width / 2);
			canvas.BeginText()
				.SetFontAndSize(_font, _fontSize)
				.SetLeading(0)
				.MoveText(underRect.GetX() + x, underRect.GetY() - _fontSize)
				.NewlineShowText(_text)
				.EndText()
				.RestoreState();
			// Use rect here
			var centerX = rect.GetLeft() + rect.GetWidth() / 2;
			var centerY = rect.GetBottom() + rect.GetHeight() / 2;
			var stampRect = new Rectangle(centerX, centerY - 55, SignatureAndStampRenderer.StampWidth, SignatureAndStampRenderer.StampHeight);
			
			var pdfDoc = drawContext.GetDocument();
			var page = pdfDoc.GetLastPage();
			canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
				.SaveState();

			if (_signature != null)
			{
				var signRect = stampRect.Clone()
				.MoveLeft(40f);

				signRect.MoveUp(_moveUpSign);

				if (_applyPadding)
				{
					signRect
					.DecreaseHeight(40f)
					.DecreaseWidth(40f);
				}
				else
				{
					signRect
					.DecreaseHeight(20f)
					.DecreaseWidth(20f);
				}
				canvas.AddXObjectFittedIntoRectangle(_signature.GetXObject(), signRect);
			}
			if (_stamp != null)
				canvas.AddXObjectFittedIntoRectangle(_stamp.GetXObject(), stampRect.MoveUp(_moveUpStamp));
			canvas.RestoreState();
		}
	}
}
