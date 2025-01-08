using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class ImageCellRenderer : CellRenderer
	{
		private readonly Image _image;
		private readonly float _width;
		private readonly TextAlignment _alignment;

		public ImageCellRenderer(Cell modelElement, Image image, float width, TextAlignment alignment = TextAlignment.JUSTIFIED) 
			: base(modelElement)
		{
			_image = image;
			_width = width;
			_alignment = alignment;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);
			
			if(_image == null)
				return;

			var rect = GetOccupiedAreaBBox();
			var height = Math.Max(rect.GetWidth(), rect.GetHeight());
			if (_alignment == TextAlignment.JUSTIFIED)
			{
				rect.ApplyMargins(-4f, -4f, -4f, -4f, false);
				rect = new Rectangle(
					rect.GetX()
					, rect.GetY()
					, height
					, height);
			}
			else
			{
				var width = Math.Max(rect.GetWidth(), _width);
				var x = rect.GetX() - (width - _width) / 2 - 6f;	
				rect = new Rectangle(x, rect.GetY() - width, width, width);
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
