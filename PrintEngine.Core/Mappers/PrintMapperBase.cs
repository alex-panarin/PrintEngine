using PrintEngine.Core.Interfaces;

namespace PrintEngine.Core.Mappers
{
    /// <summary>
    /// Осуществляет мапинг полученных с сервиса печати данных с моделью данных для печатной формы
    /// </summary>
    /// <typeparam name="TInput">Тип данных получаемых с сервиса печати</typeparam>
    /// <typeparam name="TModel">Тип модели данных в которую осуществляется маппинг</typeparam>
    public abstract class PrintMapperBase<TInput, TModel> : IPrintMapper
        where TInput : class
        where TModel : class, IPrintModel
    {
        protected IDictionary<string, string> Metadatas { get; private set; }
        IPrintModel IPrintMapper.Map(object value, IDictionary<string, string> properties)
        {
			Metadatas = properties;
			return MapInternal((TInput)value);
        }

        protected abstract TModel MapInternal(TInput inputData);
    }
}
