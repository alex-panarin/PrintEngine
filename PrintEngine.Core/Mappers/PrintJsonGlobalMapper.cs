using Newtonsoft.Json.Linq;
using PrintEngine.Core.Interfaces;

namespace PrintEngine.Core.Mappers
{
	public class PrintJsonGlobalMapper<TModel>
		: PrintJsonMapperBase<JObject, TModel>
		where TModel : class, IPrintModel, new()
	{
		protected override TModel CreateModel()
		{
			return new TModel();
		}
	}
}
