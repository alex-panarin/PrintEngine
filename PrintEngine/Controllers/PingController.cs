using Microsoft.AspNetCore.Mvc;
using PrintEngine.Core;

namespace PrintEngine.Controllers
{
	/// <summary>
	/// Контроллер состояния
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class PingController : ControllerBase
	{
		/// <summary>
		/// Возвращает текущее время и версию ядра
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var content = DateTimeOffset.Now.ToString("dd.MM.yyyy HH:mm") + Environment.NewLine
				+ typeof(PrintComposerBase).Assembly.GetName().Version;
			return await Task.FromResult(Content(content));
		}
	}
}
