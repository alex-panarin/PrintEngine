using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Mappers;
using System.Collections.Concurrent;
namespace PrintEngine.Core
{
	public class PrintMapperFactory : IPrintMapperFactory
	{
		private readonly static ConcurrentDictionary<(Type, Type), PrintMapperParams> _storage = new();

		public static void RegisterMapper(Type inputDataType, Type printModelType, Type mapperType)
		{
			_storage.TryAdd((inputDataType, printModelType), new PrintMapperParams
			{
				MapperType = mapperType,
			});
		}
		public IPrintMapper GetMapper(Type inputDataType, Type printModelType)
		{
			return _storage.GetOrAdd((inputDataType, printModelType), ((Type InputType, Type ModelType) k) => new PrintMapperParams
			{
				ModelType = k.ModelType
			})
			?.Invoke();
		}
		class PrintMapperParams
		{
			public Type ModelType;
			public Type MapperType;
			private IPrintMapper Mapper;
			public IPrintMapper Invoke()
			{
				if (Mapper == null)
				{
					var mapperType = MapperType ?? typeof(PrintJsonGlobalMapper<>).MakeGenericType(ModelType);
					Mapper = (IPrintMapper)Activator.CreateInstance(mapperType);
				}
				return Mapper;
			}

		}
	}
}
