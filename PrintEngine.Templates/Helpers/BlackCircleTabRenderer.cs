using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class BlackCircleTabRenderer : TabRenderer
	{
		private readonly float _diameter;
		private readonly float _marginTop;

		public BlackCircleTabRenderer(Tab textElement, float diameter = 2f, float marginTop = 0f) 
			: base(textElement)
		{
			_diameter = diameter;
			_marginTop = marginTop;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);
			var canvas = drawContext
				.GetCanvas()
				.SaveState()
				.SetStrokeColor(ColorConstants.BLACK);

			var rect = GetOccupiedAreaBBox().ApplyMargins(_marginTop, 0, 0, 0, false);
			var x = 5f;
			var y = _diameter / 2 ;
			//var dr = new Rectangle(rect.GetLeft() + x, rect.GetTop() - (_height * 1.6f), _width, _height);
			canvas.Circle(rect.GetLeft() + x, rect.GetTop() + 2f , y)
				.FillStroke()
				//.Stroke()
				.RestoreState();
		}
	}
}
