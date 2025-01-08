using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class FitCellRenderer : CellRenderer
	{
		private readonly string _content;

		public FitCellRenderer(Cell modelElement, string content = null)
			: base(modelElement)
		{
			_content = content;
			if (content == null)
			{
				var p = (Paragraph)modelElement.GetChildren()[0];
				foreach (var text in p.GetChildren())
				{
					_content += ((Text)text).GetText();
				}

			}
		}
		public override IRenderer GetNextRenderer()
		{
			return new FitCellRenderer((Cell)modelElement, _content);
		}
		public override LayoutResult Layout(LayoutContext layoutContext)
		{
			var font = GetPropertyAsFont(Property.FONT);
			int contentLength = _content.Length;
			int leftChar = 0;
			int rightChar = contentLength - 1;

			Rectangle rect = layoutContext.GetArea().GetBBox().Clone();

			// Cell's margins, borders and paddings should be extracted from the available width as well.
			// Note that this part of the sample was introduced specifically for iText7.
			// since in iText5 the approach of processing cells was different
			ApplyMargins(rect, false);
			ApplyBorderBox(rect, false);
			ApplyPaddings(rect, false);
			var availableWidth = rect.GetWidth();

			UnitValue fontSizeUV = this.GetPropertyAsUnitValue(Property.FONT_SIZE);

			// Unit values can be of POINT or PERCENT type. In this particular sample
			// the font size value is expected to be of POINT type.
			var fontSize = fontSizeUV.GetValue();

			availableWidth -= font.GetWidth("...", fontSize);

			while (leftChar < contentLength && rightChar != leftChar)
			{
				availableWidth -= font.GetWidth(_content[leftChar], fontSize);
				if (availableWidth > 0)
				{
					leftChar++;
				}
				else
				{
					break;
				}

				availableWidth -= font.GetWidth(_content[rightChar], fontSize);

				if (availableWidth > 0)
				{
					rightChar--;
				}
				else
				{
					break;
				}
			}

			// left char is the first char which should not be added
			// right char is the last char which should not be added
			var newContent = _content.Substring(0, leftChar) + "..." + _content.Substring(rightChar + 1);
			Paragraph p = new Paragraph(newContent);

			// We're operating on a Renderer level here, that's why we need to process a renderer,
			// created with the updated paragraph
			IRenderer pr = p.CreateRendererSubTree().SetParent(this);
			childRenderers.Add(pr);

			return base.Layout(layoutContext);
		}
	}
}
