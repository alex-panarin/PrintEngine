using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class BlackCircleCellRenderer : CellRenderer
	{
		private readonly float _diameter;

		public BlackCircleCellRenderer(Cell modelElement, float diameter = 2f) 
			: base(modelElement)
		{
			_diameter = diameter;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);

			var canvas = drawContext
				.GetCanvas()
				.SaveState()
				.SetStrokeColor(ColorConstants.BLACK);

			
			var rect = GetOccupiedAreaBBox();
			var x = rect.GetWidth() / 2;
			var y = _diameter / 2;
			//var dr = new Rectangle(rect.GetLeft() + x, rect.GetTop() - (_height * 1.6f), _width, _height);
			canvas.Circle(rect.GetLeft() + x, rect.GetTop() - (_diameter * 5f), y)
				.FillStroke()
				//.Stroke()
				.RestoreState();
		}
	}
}