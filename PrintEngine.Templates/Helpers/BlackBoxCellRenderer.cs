using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class BlackBoxCellRenderer : CellRenderer
	{
		private readonly float _width;
		private readonly float _height;
		private readonly bool _useVMargin;
		private readonly bool _useHMargin;

		public BlackBoxCellRenderer(Cell modelElement, float width = 10f, float height = 10f, bool useVMargin = true, bool useHMargin = true)
			: base(modelElement)
		{
			_width = width;
			_height = height;
			_useVMargin = useVMargin;
			_useHMargin = useHMargin;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);
			
			var canvas = drawContext
				.GetCanvas()
				.SaveState()
				.SetStrokeColor(ColorConstants.BLACK)
				.SetLineWidth(0.6f);
			//var area = GetOccupiedArea();
			var rect = GetOccupiedAreaBBox();
			var x = rect.GetWidth() / 2;
			x -= _width / 2;
			var dr = new Rectangle(rect.GetLeft() + (_useHMargin ? x : (x / 2) )
				, _useVMargin ? (rect.GetTop() - _height * 1.6f ): (rect.GetTop() - _height * 1.2f)
				, _width
				, _height);
			canvas.Rectangle(dr)
				.Stroke()
				.RestoreState();
		}
	} 
}
