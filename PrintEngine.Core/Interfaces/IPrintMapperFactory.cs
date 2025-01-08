using System.Text.Json.Nodes;

namespace PrintEngine.Core.Interfaces
{
	public interface IPrintMapperFactory
	{
		IPrintMapper GetMapper(Type inputDataType, Type printModelType);
	}
}
