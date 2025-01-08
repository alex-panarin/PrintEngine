using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintEngine.Core;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using PrintEngine.Examples;
using PrintEngine.Extentions;
using Swashbuckle.AspNetCore.Filters;

namespace PrintEngine.Controllers
{
	/// <summary>
	/// Класс контроллера печати
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class PrintEngineController : ControllerBase
	{
		private readonly IPrintComposer _composer;
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="composer"></param>
		/// <param name="loggerFactory"></param>
		/// <param name="configuration"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public PrintEngineController(IPrintComposer composer
			, ILoggerFactory loggerFactory
			, IConfiguration configuration)
		{
			_composer = composer ?? throw new ArgumentNullException(nameof(composer));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_logger = loggerFactory.CreateLogger<PrintEngineController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}
		/// <summary>
		/// Возвращает печатную форму
		/// </summary>
		/// <remarks>
		/// Пример запроса:
		///		
		///		POST /Engage
		///		{
		///			"TemplateId":"string (обязателен)",
		///			"MetadataId":"string (возможен)",
		///			"NeedSign":bool (возможен) если не указан, то false, 
		///			"CorrelationId":"string($uuid) (возможен) если не указан, формируется автоматически",
		///			"Data":{ модель входных данных } (обязателен)
		///		}	
		///		
		/// </remarks>
		/// <param name="inputObject">Json формата, описанного в примере выше</param>
		/// <returns>Возвращает объект, содержащий печатную форму</returns>
		/// <response code="400">Возвращает объект, содержащий описание ошибки</response>
		/// <response code="200">Возвращает объект, содержащий печатную форму</response>
		[HttpPost(nameof(Engage))]
		[ProducesResponseType(typeof(ActionResult<PrintResult>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ActionResult<PrintResult>), StatusCodes.Status400BadRequest)]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(EngageResponseExampleOk))]
		[SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(EngageResponseExampleBadRequest))]
		[SwaggerRequestExample(typeof(JObject), typeof(EngageRequestExample))]
		public async Task<IActionResult> Engage([FromBody] JObject inputObject)
		{
			//var logData = new LogData
			//{
			//	Action = $"{nameof(PrintEngineController)}.{nameof(Engage)}"
			//};

			inputObject.TryGetValue("CorrelationId", StringComparison.InvariantCultureIgnoreCase, out var correlationNode);
			var correlationId = correlationNode?.ToObject<Guid?>() ?? Guid.NewGuid();

			try
			{
				//logData.CorrelationId = correlationId;

				if (!inputObject.TryGetValue("TemplateId", StringComparison.InvariantCultureIgnoreCase, out var templateNode))
					throw new Exception($"{Errors.APIDOTNET_22_3} => {nameof(templateNode)}");

				var templateId = templateNode?.ToObject<string>();
				if (templateId == null)
					throw new Exception($"{Errors.APIDOTNET_22_3} => {nameof(templateId)}");

				inputObject.TryGetValue("NeedSign", StringComparison.InvariantCultureIgnoreCase, out var signNode);
				var needSign = signNode?.ToObject<bool?>() ?? false;

				var metadataId = templateId;
				if (inputObject.TryGetValue("MetadataId", StringComparison.InvariantCultureIgnoreCase, out var metadataNode)
					&& metadataNode?.ToObject<string>() is var md)
					metadataId = string.IsNullOrEmpty(md) ? metadataId : md;

				if (!inputObject.TryGetValue("data", StringComparison.InvariantCultureIgnoreCase, out var data))
					throw new Exception($"{Errors.APIDOTNET_22_4} => {nameof(data)}");

				var fileName = _composer.GetTemplateFileName(templateId);
				if (fileName != null)
					inputObject.AddFirst(new JProperty("TemplateFile", $"{fileName}.pub"));

				var request = new ComposeRequest
				{
					MetadataName = metadataId,
					TemplateName = templateId,
					JsonValue = JsonConvert.DeserializeObject<JObject>(data.ToString()),
					NeedSign = needSign
				};

				//_logger.StartOperation(logData, inputObject.ToString().Replace("\\\"", "\""));

				var result = await _composer.Compose(request, correlationId, _configuration.IsTestEnvironment());

				//if (result.IsSuccess)
				//	_logger.CompleteOperation(logData, result.ToString());
				//else
				//	_logger.Error(logData, result.Error);

				return Ok(result);
			}
			catch (JsonException x)
			{
				//_logger.Error(logData, x.ToString());
				return BadRequest(PrintResult.Fail($"{Errors.APIDOTNET_22_5} => {x.Message}", correlationId));
			}
			catch (FormatException x)
			{
				//_logger.Error(logData, x.ToString());
				return BadRequest(PrintResult.Fail($"{Errors.APIDOTNET_22_2} => {x.Message}", correlationId));
			}
			catch (Exception x)
			{
				//_logger.Error(logData, x.ToString());
				return BadRequest(PrintResult.Fail(x.Message, correlationId));
			}
		}
		/// <summary>
		/// Список шаблонов для печати
		/// </summary>
		/// <returns></returns>
		[HttpGet(nameof(Templates))]
		[ProducesResponseType(typeof(ActionResult<TemplateResult>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Templates()
		{
			//var logData = new LogData
			//{
			//	Action = "PrintEngineController.Templates",
			//	CorrelationId = Guid.NewGuid()
			//};
			try
			{
				return Ok(await _composer.GetTemplates());
			}
			catch (Exception x)
			{
				//_logger.Error(logData, x.ToString());
				return BadRequest(TemplateResult.Fail(x.Message));
			}
		}
	}
}
