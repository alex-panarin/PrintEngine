using Newtonsoft.Json.Linq;
using PrintEngine.Core.Interfaces;

namespace PrintEngine.Core.Mappers
{
    public abstract class PrintJsonMapperBase<TInput, TModel> : PrintMapperBase<TInput, TModel>
        where TModel : class, IPrintModel
        where TInput : JObject
    {
        protected abstract TModel CreateModel();
        protected override TModel MapInternal(TInput source)
        {
            if (Metadatas == null)
                throw new ArgumentNullException(nameof(Metadatas));

            var target = CreateModel();
            foreach (var data in Metadatas)
            {
                var value = source.SelectToken(data.Value);
                if (value == null)
                    value = FindValue(source.Properties(), data.Value);

                target.SetValue(value, data.Key);
            }
            return target;
        }
        private JToken FindValue(IEnumerable<JProperty> values, string name)
        {
            var prop = name.Any(n => n == '.')
                ? values.FirstOrDefault(v => v.Path?.Contains(name, StringComparison.CurrentCultureIgnoreCase) == true)
                : values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (prop != null)
                return prop.Value;

            foreach (var value in values)
            {
                var token = value.SelectToken(name);
                if (token == null && value.Value is JObject obj)
                    token = FindValue(obj.Properties().Where(p => p != null), name);

                if (token != null)
                    return token;
            }
            return null;
        }
    }
}
