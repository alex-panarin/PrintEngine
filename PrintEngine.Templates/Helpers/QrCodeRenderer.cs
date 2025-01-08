using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class QrCodeRenderer : CellRenderer
	{
		private readonly Image _image;
		private readonly Color _background;

		public QrCodeRenderer(Cell modelElement, Image image, Color background = null) 
			: base(modelElement)
		{
			_image = image;
			_background = background;
		}

		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);
			var rect = GetOccupiedAreaBBox().ApplyMargins(-4f, -4f, -4f, -4f, false).MoveUp(4f).MoveLeft(12f);
			var height = Math.Max(rect.GetWidth(), rect.GetHeight());
			rect = new Rectangle(
				rect.GetX()
				, rect.GetY()
				, height
				, height);
			if (_background != null)
			{
				drawContext
					.GetCanvas()
					.SaveState()
					.Rectangle(rect.Clone().ApplyMargins(6f, 6f, 6f, 6f, false))
					.SetStrokeColor(_background)
					.SetFillColor(_background)
					.FillStroke()
					.RestoreState();
			}
			drawContext
				.GetCanvas()
				.SaveState()
				.AddXObjectFittedIntoRectangle(_image.GetXObject(), rect)
				.Stroke()
				.RestoreState();
		}
	}
}
