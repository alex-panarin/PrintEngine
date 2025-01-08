using Microsoft.AspNetCore.Mvc;
using PrintEngine.Core.Interfaces;
using PrintEngine.Metadata.Interfaces;

namespace PrintEngine.Controllers
{
	/// <summary>
	/// Контроллер метаданных
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class MetadataController : Controller
	{
		private readonly IPrintComposer _composer;
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="composer"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public MetadataController(IPrintComposer composer)
		{
			_composer = composer ?? throw new ArgumentNullException(nameof(composer));
		}
		/// <summary>
		/// Получаем метаданные для шаблона
		/// </summary>
		/// <returns></returns>
		[HttpGet("template={templateId}")]
		[ProducesResponseType(typeof(ActionResult<IDictionary<string, string>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ActionResult<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetMapping(string templateId)
		{
			if (_composer is not IMetadataExplorer explorer)
				return BadRequest("Не возможно получить метаданнные");

			try
			{
				return Ok(await explorer.GetMetadataAsync(templateId));
			}
			catch (Exception x)
			{
				return BadRequest(x.Message);
			}
		}

	}
}
