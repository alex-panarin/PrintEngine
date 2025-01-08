using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class UnderlineCellRenderer : CellRenderer
	{
		private readonly bool _useTwoLine;
		protected readonly float _width;

		public UnderlineCellRenderer(Cell modelElement, bool useTwoLine, float width = 0.5f)
			: base(modelElement)
		{
			_useTwoLine = useTwoLine;
			this._width = width;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);

			var canvas = drawContext
				.GetCanvas()
				.SaveState()
				.SetLineWidth(_width);

			var rect = GetOccupiedAreaBBox();
			
			if (_useTwoLine)
			{
				var numberOfRow = (int)Math.Round(rect.GetHeight() / 14, 0);
				var up = rect.GetHeight() / numberOfRow;

				for (var i = 0; i < numberOfRow; i ++)
				{
					var r = rect.Clone()
					.MoveUp(up * i - 2)
					.ApplyMargins(0, 0, i == 0 ? 4f : 2f, 0, false);
					canvas.MoveTo(r.GetLeft(), r.GetBottom())
						.LineTo(r.GetRight(), r.GetBottom())
						.Stroke();
				}
			}
			else
			{
				rect.ApplyMargins(0, 0, 2f, 0, false);
				canvas.MoveTo(rect.GetLeft(), rect.GetBottom())
					.LineTo(rect.GetRight(), rect.GetBottom())
					.Stroke();
			}
			canvas.RestoreState();
		}
	}
}
