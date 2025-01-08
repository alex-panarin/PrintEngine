using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using PrintEngine.Core;

namespace PrintEngine.Templates.Helpers
{
    public class PdfNewPageEventHanler : IEventHandler
    {
        private readonly bool _printDemonstration;

        public PdfNewPageEventHanler(bool printDemonstration)
        {
            _printDemonstration = printDemonstration;
        }
        public virtual void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            if (docEvent == null || _printDemonstration == false)
                return;

            var page = docEvent.GetPage();
            var font = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            var rect = page.GetPageSize().Clone().ApplyMargins(11f, 0, 0, 60f, false);
            var text = $"Demonstration Powered by RGSPrintService {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}," +
                $" Version: {typeof(PrintComposerBase).Assembly.GetName().Version}";
            var canvas = new PdfCanvas(page);
            canvas
                .SaveState()
                .BeginText()
                .SetFontAndSize(font, 12f)
                .MoveText(rect.GetLeft(), rect.GetTop())
                .ShowText(text)
                .EndText()
                .RestoreState()
                ;
        }
    }
}