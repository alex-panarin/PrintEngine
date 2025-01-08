using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace PrintEngine.Templates.Helpers
{
    internal static class CellExtentions
	{
	
		public static Cell SetBackgroundGrayColor(this Cell cell, bool useColor = true, bool topWhite = false)
		{
			if (useColor)
			{
				cell.SetBackgroundColor(Utils.FillColor)
					.SetNoBorder();
				if (topWhite)
					cell.SetBorderTop(new SolidBorder(ColorConstants.WHITE, 0.5f));
				return cell;
			}
			return cell.SetLightGrayBorder();
		}
		public static Table SetBackgroundGrayColor(this Table table)
		{
			return table.SetBackgroundColor(Utils.FillColor);
		}
		public static Cell SetUnderlineRenderer(this Cell cell, bool useToLine = false, float width = 0.5f)
		{
			cell.SetNextRenderer(new UnderlineCellRenderer(cell, useToLine, width));
			return cell;
		}
		public static Text SetUnderlineRenderer(this Text text, bool useParentCell = true, float width = 0.5f)
		{
			text.SetNextRenderer(new UnderlineTextRenderer(text, useParentCell, width));
			return text;
		}
		public static Text SetBorderBoxRenderer(this Text text, bool fillCross, bool drawBorder = true, float vMargin = 0)
		{
			text.SetFontColor(ColorConstants.WHITE)
				.SetNextRenderer(new BorderBoxTextRenderer(text, fillCross, drawBorder, vMargin));
			return text;
		}
		public static Cell SetUnderlineRendererText(this Cell cell, string text, PdfFont font, float yOffset = 0f,float width = 0.5f, float fontSize = 6f, TextAlignment alignment = TextAlignment.CENTER)
		{
			cell.SetNextRenderer(new UnderlineRendererText(cell, text, font, yOffset, width, fontSize, alignment));
			return cell;
		}
        public static Cell SetUnderlineRendererText(this Cell cell, Paragraph[] paragraphs, PdfFont font, float yOffset = 0f, float width = 0.5f, float fontSize = 6f, TextAlignment alignment = TextAlignment.CENTER)
        {
            cell.SetNextRenderer(new UnderlineRendererText(cell, paragraphs, font, yOffset, width, fontSize, alignment));
            return cell;
        }
        public static Cell SetQrCodeRenderer(this Cell cell, Image image, Color background = null)
		{
			cell.SetNextRenderer(new QrCodeRenderer(cell, image, background));
			return cell;
		}
		public static Cell SetImageRenderer(this Cell cell, Image image, float width)
		{
			cell.SetNextRenderer(new ImageCellRenderer(cell, image, width, TextAlignment.CENTER));
			return cell;
		}
		public static Paragraph SetImageRenderer(this Paragraph para, Image image)
		{
			para.SetNextRenderer(new ImageParagraphRenderer(para, image));
			return para;
		}
		public static Cell SetUnderlineRendererTextAndStamp(this Cell cell, string text, Image image, PdfFont font)
		{
			cell.SetNextRenderer(new UnderlineRendererTextAndStamp(cell, image, text, font));
			return cell;
		}
		public static Cell SetSignatureAndStampRenderer(this Cell cell, ImageData signature, ImageData stamp, bool isDraft = false, SignatureAndStampParams param = null)
		{
			if(isDraft == false)
				cell.SetNextRenderer(new SignatureAndStampRenderer(cell, signature, stamp, param ?? new SignatureAndStampParams()));
			return cell;
		}
		public static Cell SetUnderlineSignatureStampAndTextRenderer(this Cell cell, Image signature, Image stamp, string text, PdfFont font, bool applyPadding = false,  bool isDraft = false, float moveUpSign = 20f, float moveUpStamp = 10f)
		{
			cell.SetNextRenderer(new UnderlineRendererSignatureStampText(cell, isDraft ? null : signature, isDraft ? null : stamp, text, font, applyPadding, moveUpSign, moveUpStamp));
			return cell;
		}
		public static Cell SetNoBorder(this Cell cell)
		{
			cell.SetBorder(Border.NO_BORDER);
			return cell;
		}
		public static Cell SetGrayBorder(this Cell cell)
		{
			cell.SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f));
			return cell;
		}
        public static Cell SetBlackBold(this Cell cell)
        {
            cell.SetBorder(new SolidBorder(ColorConstants.BLACK, 0.8f));
            return cell;
        }
        public static Cell SetLightGrayBorder(this Cell cell)
		{
			cell.SetBorder(Utils.LightGrayBorder);
			return cell;
		}
		public static Paragraph SetLightGrayBorder(this Paragraph para, bool border = false)
		{
			return para
				.SetBorder(new SolidBorder(border ? Utils.FillColor : ColorConstants.WHITE, 0.5f) )
				.SetBackgroundColor(ColorConstants.WHITE)
				.SetMarginTop(0)
				.SetMarginBottom(0)
				.SetPaddingBottom(0)
				.SetPaddingTop(0);
		}
		public static Table SetGrayBorder(this Table table)
		{
			table.SetBorder(Utils.LightGrayBorder);
			return table;
		}

        public static Table SetBlackBold(this Table table)
        {
            table.SetBorder(Utils.BlackBorder);
            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="wight">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="useVMargin">Вертикальное смещение</param>
        /// <param name="useHMargin">Горизонтальное смещение</param>
        /// <returns></returns>
        public static Cell SetBlackBoxRenderer(this Cell cell, float wight = 10f, float height = 10f, bool useVMargin = true, bool useHMargin = true)
		{
			cell.SetNextRenderer(new BlackBoxCellRenderer(cell, wight, height, useVMargin, useHMargin));
			return cell;
		}
		public static Paragraph SetBlackBoxRenderer(this Paragraph para, float wight = 10f, float height = 10f, bool useVMargin = true, bool useHMargin = true)
		{
			para.SetNextRenderer(new BlackBoxParagraphRenderer(para, wight, height, useVMargin, useHMargin));
			return para;
		}
		public static Cell SetBlackCircleRenderer(this Cell cell, float diameter = 2f)
		{
			cell.SetNextRenderer(new BlackCircleCellRenderer(cell, diameter));
			return cell;
		}
		public static Tab SetBlackCircleTabRenderer(this Tab text, float diameter = 2f, float marginTop = 0f)
		{
			text.SetNextRenderer(new BlackCircleTabRenderer(text, diameter, marginTop));
			return text;
		}
		/// <summary>
		/// Вызывать в самом конце процедуры заполнения разметки
		/// </summary>
		/// <param name="layout">разметка</param>
		/// <param name="fontSize">размер шрифта</param>
		/// <param name="alignment">расположение</param>
		/// <returns></returns>
		public static Document SetPageNumber(this Document layout, float fontSize, TextAlignment alignment)
		{
			var doc = layout.GetPdfDocument();
			var pages = doc.GetNumberOfPages();
			var size = doc.GetDefaultPageSize();
			var font = doc.GetDefaultFont();
			var width = font.GetContentWidth(new PdfString($"Страница 1 из {pages}", Utils.CyrEncoding)) / 1000 * fontSize;
			var x = size.GetLeft();
			if (alignment == TextAlignment.RIGHT)
				x = size.GetRight() - width /2;
			if(alignment == TextAlignment.CENTER)
				x = size.GetLeft() + (size.GetWidth() / 2 - width / 2);
			for (int i = 1; i <= pages; i ++)
			{
				layout.ShowTextAligned(new Paragraph($"Страница {i} из {pages}").SetFontSize(fontSize).SetFontColor(ColorConstants.GRAY),
					x, size.GetBottom() + 20f, i, alignment, VerticalAlignment.BOTTOM, 0 );
			}
			return layout;

		}
	}
}
