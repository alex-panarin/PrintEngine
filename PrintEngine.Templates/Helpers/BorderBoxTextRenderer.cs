using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class BorderBoxTextRenderer
		: TextRenderer
	{
		private readonly bool _fillCross;
		private readonly bool _drawBorder;
		private readonly float _vMargin;
		private readonly bool _useParentCell;

		public BorderBoxTextRenderer(Text textElement, bool fillCross, bool drawBorder = true, float vMargin = 0f,  bool useParentCell = true)
			: base(textElement)
		{
			_fillCross = fillCross;
			_drawBorder = drawBorder;
			_vMargin = vMargin;
			_useParentCell = useParentCell;
		}
		private CellRenderer GetCellRenderer()
		{
			var element = this.parent;
			while (element is not CellRenderer)
				element = element.GetParent();

			return (CellRenderer)element;
		}

		private ParagraphRenderer GetParagraphRenderer()
		{
			var element = this.parent;
			while (element is not ParagraphRenderer)
				element = element.GetParent();

			return (ParagraphRenderer)element;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);
			const float width = 8f;
			BlockRenderer renderer = _useParentCell
				? GetCellRenderer()
				: GetParagraphRenderer();
			var cellRect = renderer.GetOccupiedAreaBBox().ApplyMargins(_vMargin + 2f, 2f, 2f, 2f, false);
			var rect = new Rectangle(cellRect.GetLeft(), cellRect.GetTop() - width, width, width);
			var canvas =
				drawContext
				.GetCanvas()
				.SaveState()
				.SetFillColor(ColorConstants.WHITE)
				.Rectangle(rect)
				.Fill()
				.RestoreState()
				.SaveState();
			if (_drawBorder)
			{
				canvas
				.SetLineWidth(0.5f)
				.MoveTo(cellRect.GetLeft(), cellRect.GetBottom())
				.Rectangle(rect)
				.Stroke();
			}

			if (_fillCross) // Draw X
			{
				if (((Text)modelElement).GetText() == "X")
				{
					canvas
						.SetLineWidth(0.25f)
						.MoveTo(rect.GetLeft(), rect.GetBottom())
						.LineTo(rect.GetRight(), rect.GetTop())
						.Stroke()
						.MoveTo(rect.GetLeft(), rect.GetTop())
						.LineTo(rect.GetRight(), rect.GetBottom())
						.Stroke();
				}
				else // Draw V
				{
					canvas
						.SetLineWidth(0.25f)
						.MoveTo(rect.GetLeft() + 1, rect.GetTop() - 1)
						.LineTo(rect.GetLeft()+ rect.GetWidth() / 2, rect.GetBottom() + 1)
						.Stroke()
						.MoveTo(rect.GetLeft()+ rect.GetWidth() / 2, rect.GetBottom() + 1)
						.LineTo(rect.GetRight() - 1, rect.GetTop() - 1)
						.Stroke();
				}
			}
			canvas
				.RestoreState();
		}
	}
}