using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintEngine.Core.Interfaces;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PrintEngine.Core.Models
{
	public abstract class PrintModelBase : IPrintModel
	{
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			Culture = CultureInfo.GetCultureInfo("ru-RU"),
			DateParseHandling = DateParseHandling.None,
		};
		private readonly Dictionary<string, object> _cache = new();

		protected T GetValue<T>([CallerMemberName] string name = null)
		{
			if (!_cache.TryGetValue(name, out var value) || value == null)
				return default(T);

			if (value is JToken modelValue)
			{
				_cache[name] = value = JsonConvert
					.DeserializeObject<T>(JsonConvert
					.SerializeObject(modelValue), SerializerSettings);
			}

			return (T)value;
		}
		protected void SetValue<T>(T value, [CallerMemberName] string name = null)
		{
			if (value is JToken)
			{
				_cache.TryAdd(name, value);
				return;
			}
			_cache[name] = value; // Update value in cache
		}
		protected abstract void Validate();
		T IPrintModel.GetValue<T>(string name) => GetValue<T>(name);
		void IPrintModel.SetValue(object value, string name) => SetValue(value, name);
		void IPrintModel.Validate() => Validate();
	}
}
