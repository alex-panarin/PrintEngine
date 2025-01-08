using iText.IO.Image;
using iText.Kernel.Font;
using PrintEngine.Core.Interfaces;

namespace PrintEngine.Templates.Helpers
{
    internal static class ResouceExtentions
	{
		const string ARIAL = "arial";
		const string ARIALBOLD = "arialbd";
		const string ARIALITALIC = "ariali";
		const string TIMES = "times";
		const string TIMESBOLD = "timesbd";
		const string TIMESITALIC = "timesi";
		const string WINGDING = "wingding";
		const string WEBDING = "webdings";
		internal static PdfFont GetArial(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(ARIAL);
		}
		internal static PdfFont GetArialBold(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(ARIALBOLD);
		}
		internal static PdfFont GetArialItalic(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(ARIALITALIC);
		}
		internal static PdfFont GetTimes(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(TIMES);
		}
		internal static PdfFont GetTimesBold(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(TIMESBOLD);
		}
		internal static PdfFont GetTimesItalic(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(TIMESITALIC);
		}
		internal static PdfFont GetWingDing(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(WINGDING);
		}
		internal static PdfFont GetWebDing(this IResourceService<ImageData, PdfFont> service)
		{
			return service.GetFont(WEBDING);
		}
	}
}
