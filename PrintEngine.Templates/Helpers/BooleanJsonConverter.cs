using Newtonsoft.Json;

namespace PrintEngine.Templates.Helpers
{
	public class BooleanJsonConverter : JsonConverter
	{
		public override bool CanWrite => false;
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var value = reader.Value?.ToString();
			return value == "1" 
				|| string.Compare(value, "true", StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool)
				|| objectType == typeof(bool?);
		}

	}
}
