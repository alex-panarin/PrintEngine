using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrintEngine.Templates.Helpers
{
	public class SingleOrListJsonConverter<T> : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(T) 
				|| objectType == typeof(List<T>);	
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.Load(reader);
			
			if (token.Type == JTokenType.Array)
				return token.ToObject<List<T>>(serializer);

			return new List<T> { token.ToObject<T>(serializer) };
		}

		public override bool CanWrite => false;
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
