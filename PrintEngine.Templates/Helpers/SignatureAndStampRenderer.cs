using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace PrintEngine.Templates.Helpers
{
	internal class SignatureAndStampParams
	{
		public float moveUpSign = 0f;
		public float moveUpStamp = 10f;
		public float moveLeftSign = 35f;
		public float moveleftStamp = 0f;
		public int pageNumber = 0;
	}
	internal class SignatureAndStampRenderer : CellRenderer
	{
		public const float StampWidth = 120f;
		public const float StampHeight = 120f;
		private readonly Image _signature;
		private readonly Image _stamp;
		private readonly float _moveUpSign;
		private readonly float _moveUpStamp;
		private readonly float _moveLeftSign;
		private readonly float _moveleftStamp;
		private readonly int _pageNumber;

		public SignatureAndStampRenderer(Cell modelElement, ImageData signature, ImageData stamp, SignatureAndStampParams param)
			: base(modelElement)
		{
			_signature = signature != null ? new Image(signature) : null;
			_stamp = stamp != null ? new Image(stamp) : null;
			_moveUpSign = param.moveUpSign;
			_moveUpStamp = param.moveUpStamp;
			_moveLeftSign = param.moveLeftSign;
			_moveleftStamp = param.moveleftStamp;
			_pageNumber = param.pageNumber;
		}
		public override void Draw(DrawContext drawContext)
		{
			base.Draw(drawContext);

			var rect = GetOccupiedAreaBBox();
			var centerX = rect.GetLeft() + rect.GetWidth() / 2;
			var centerY = rect.GetBottom() + rect.GetHeight() / 2;
			var imageRect = new Rectangle(centerX - (_signature == null ? 35f : 25f), centerY - 55, StampWidth, StampHeight);
			
			var pdfDoc = drawContext.GetDocument();
			var page = _pageNumber == 0 ? pdfDoc.GetLastPage() : pdfDoc.GetPage(_pageNumber) ;
			var canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
				.SaveState();

			if (_signature != null)
			{
				var imageWidth = _signature.GetImageWidth();
				var imageHeight = _signature.GetImageHeight();
				var ratio = imageWidth / imageHeight;
				var signRect = ratio < 2.0f
					? imageRect.Clone().DecreaseHeight(20f).DecreaseWidth(15f)
					: new Rectangle(imageRect.GetX(), imageRect.GetY(), imageWidth/3.5f, imageHeight/3.5f);
				canvas.AddXObjectFittedIntoRectangle(_signature.GetXObject(), signRect.MoveUp(_moveUpSign).MoveLeft(_moveLeftSign));
			}
			
			if (_stamp != null)
				canvas.AddXObjectFittedIntoRectangle(_stamp.GetXObject(), imageRect.MoveUp(_moveUpStamp).MoveLeft(_moveleftStamp));

			canvas.RestoreState();
		}
	}
}
