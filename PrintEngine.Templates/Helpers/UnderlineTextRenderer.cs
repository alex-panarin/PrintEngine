using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class UnderlineTextRenderer
		: TextRenderer
	{
		private readonly bool _useParentCell;
		private readonly float _width;

		public UnderlineTextRenderer(Text textElement, bool useParentCell, float width = 0.5f)
			: base(textElement)
		{
			_useParentCell = useParentCell;
			_width = width;
		}

		private CellRenderer GetCellRenderer()
		{
			var	element = this.parent;
			while(element is not CellRenderer)
				element = element.GetParent();

			return (CellRenderer) element;
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

			BlockRenderer renderer = _useParentCell 
				? GetCellRenderer() 
				: GetParagraphRenderer();

			var cellRect = renderer.GetOccupiedAreaBBox();
			var rect = GetOccupiedAreaBBox()
				.SetY(cellRect.GetY())
				.SetHeight(cellRect.GetHeight())
				.ApplyMargins(0, 0, _useParentCell ? 2f : 6f, 0, false);

			drawContext
				.GetCanvas()
				.SaveState()
				.SetLineWidth(_width)
				.MoveTo(rect.GetLeft(), rect.GetBottom())
				.LineTo(rect.GetRight(), rect.GetBottom())
				.Stroke()
				.RestoreState();
		}
	}
}
