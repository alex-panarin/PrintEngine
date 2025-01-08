using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using PrintEngine.Resources;
using PrintEngine.Templates.UserModels;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace PrintEngine.Templates.Helpers
{
	public static class Utils
	{
		internal enum PolicyType
		{
			ORIGINAL,
			SAMPLE,
			COPY
		};
		internal static readonly string CyrEncoding = FontEncoding.CreateFontEncoding("cp1251").GetBaseEncoding();
		public static readonly CultureInfo RuCulture = CultureInfo.GetCultureInfo("ru-RU");
		internal static Border BlackBorder => new SolidBorder(ColorConstants.BLACK, 0.5f);
		internal static Border WhiteBorder => new SolidBorder(ColorConstants.WHITE, 0.5f);
		internal static Border LightGrayBorder => new SolidBorder(FillColor, 0.5f);
		internal static readonly Color RgsTextColor = new DeviceRgb(82, 86, 91);
		internal static readonly Color FillColor = new DeviceRgb(230, 230, 230);
		internal static bool IsUTSProgram(string programCode) => new[]
		{
			"1304", "1305", "1306", "1307",
			"1308", "1309", "1310", "1311",
			"1312", "1313", "1314", "1315"
		}.All(c => programCode != c);
		private static CultureInfo[] cultures = new[] { RuCulture, CultureInfo.InvariantCulture };
		internal static float MillimitersToPoints(float value)
		{
			return (value / 25.4f) * 72f;
		}
		internal static Cell CreateBox(string text, float fix = 6f, Border border = null)
		{
			return new Cell()
				.Add(new Paragraph(text)
				.SetFixedLeading(fix))
				.SetBorder(border ?? Border.NO_BORDER);
		}
		internal static Cell CreateBorderBox(string text, Border border, float size = 10f, float margin = 2f, float fix = 6f)
		{
			var box = CreateBox(text, fix, border);
			return box.SetFontSize(size + margin)
				.SetWidth(size)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetVerticalAlignment(VerticalAlignment.MIDDLE);
		}
		
		internal static bool IsEmpty<T>(this IEnumerable<T> array)
		{
			return array == null
				|| !array.Any();
		}
		internal static bool IsEmpty(this string value)
		{
			return string.IsNullOrWhiteSpace(value)
				|| value == "0"
				|| value == "0.00";
		}
		internal static string GetDateFormat(string date, string format = "dd.MM.yyyy" , bool emptyIfNull = false)
		{
			var dateTime = Parse(date);

			return dateTime == null 
				? (emptyIfNull ? "" : "--------") 
				: dateTime.Value.ToString(format, RuCulture);
		}
		internal static string GetDateFormat(DateTimeOffset? date, string format = "dd.MM.yyyy")
		{
			if(date == null)
				return string.Empty;
			return date.Value.ToString(format, RuCulture);
		}
		internal static string[] GetDateDigits(string date)
		{
			return GetDateFormat(date, "dd.MM.yyyy")
				.Replace(".", "")
				.Select(c => c.ToString())
				.ToArray();
		}
		internal static string[] GetDateDigits(DateTimeOffset? date)
		{
			return (date == null ? "--------" : date?.ToString("dd.MM.yyyy"))
				.Replace(".", "")
				.Select(c => c.ToString())
				.ToArray();
		}
		internal static string[] GetYearDateTime(DateTimeOffset? date)
		{
			var list = new[] { ' ', '.', ':' };
			return (date == null ? "----------" : date?.ToString("HH:mm dd.MM.yy"))
				.Where(c => ! list.Contains(c))
				.Select(c => c.ToString())
				.ToArray ();
		}
		internal static string[] GetYearDateTime(string date)
		{
			return GetYearDateTime(Parse(date));
		}
		internal static float GetContentRatio(string content, PdfFont font, float fontSize, float enoughWidth)
		{
			var width = //fontB.GetContentWidth(new PdfString(station.Address, Utils.CyrEncoding)) / 1000 * fontSize;
							font.GetWidth(content) / 1000 * fontSize;
			return (float)Math.Round(width / enoughWidth, 1);
		}
		internal static DateTimeOffset? Parse(string date)
		{
			if (date.IsEmpty() == false)
			{
				foreach (var c in cultures)
				{
					try
					{
						return DateTimeOffset.Parse(date, c);
					}
					catch
					{
					}
				}
			}
			return null;
		}
		internal static void AddWatermarkText(WatermarkProperties props, PdfPage page)
		{
			if (props.PrintWatermark == false)
				return;

			var pageNumber = props.Document.GetPdfDocument().GetPageNumber(page);
			var text = new Paragraph(props.Text)
				.SetFont(props.Font)
				.SetFontSize(props.FontSize)
				.SetCharacterSpacing(10f)
				.SetFontColor(props.GrayColor == false
					? ColorConstants.DARK_GRAY
					: ColorConstants.LIGHT_GRAY);
			if (props.Condenced)
				text.SetCharacterSpacing(-0.3f);

			var gs1 = new PdfExtGState().SetFillOpacity(0.4f);
			var size = page.GetPageSize();

			var canvas = new PdfCanvas(page);
			canvas.SaveState();
			canvas.SetExtGState(gs1);
			var x = size.GetWidth() / 2;
			var y = size.GetHeight() / 2;
			props.Document.ShowTextAligned(text, x, y, pageNumber, 
				TextAlignment.CENTER, VerticalAlignment.MIDDLE, props.TopToRight ? -0.25f * (float)Math.PI : 45);
			canvas.RestoreState();
		}
		internal static void InjectSignToDocument(this Canvas canvas, PdfFont font, float left, float bottom, float width, byte[] certificate)
		{
			var table = new Table(1)
				.UseAllAvailableWidth()
				.SetFixedLayout()
				.AddCell(new Cell().Add(new Paragraph()
					.Add(new Text("Данный полис подписан усиленной электронной подписью").SetFontSize(10).SetBold())
					.Add(new Text("\r\n\r\n--- BEGIN CERTIFICATE---\r\n").SetFontSize(5))
					.Add(new Text(Encoding.UTF8.GetString(certificate).Replace("\n", "")).SetFontSize(4f))
					.Add(new Text("\r\n--- END CERTIFICATE---\r\n").SetFontSize(5))
			.SetFixedLeading(6)
			.SetTextAlignment(TextAlignment.JUSTIFIED)
			.SetFont(font))
			.SetNoBorder())
			.SetFixedPosition(left, bottom, width);
			canvas.Add(table);
		}

        internal static int? GetTotalYears(string drivingStartExperienceDate)
        {
			var dt = Parse(drivingStartExperienceDate);
			if (dt == null) return null;

			var now = DateTimeOffset.Now;
			var years = now.Year - dt.Value.Year;

			return dt.Value.AddYears(years) > now ? years-- : years;

        }

        internal static string GetFranchise(string code, string summ, string measure) =>
            (code) switch
            {
                "2.2" => $"Безусловная со второго случая {summ} (руб.)",
                "2.3" => "динамическая",
                "2.1" => $"безусловная {summ} {(measure.IsEmpty() ? "" : $"({measure})")}",
                _ => "не установлена",
            };
       
		internal static Table AddCells(this Table table, IEnumerable<Cell> cells, Action<Cell> cellProps = null)
		{
			if (cells.IsEmpty() == false)
			{
				foreach (var cell in cells)
				{
					cellProps?.Invoke(cell);
					table.AddCell(cell);
				}
			}
			return table;
		}

        internal static Table AddCells(this Table table, IEnumerable<Paragraph> paras, Action<Paragraph> paraProps = null, Action<Cell> cellProps = null)
		{
			if(paras.IsEmpty() == true) return table;	
			
			var cells = paras.Select(p =>
			{
				paraProps?.Invoke(p);
				return new Cell().Add(p); 
			});

			return table.AddCells(cells, cellProps);
		}
    }
}
