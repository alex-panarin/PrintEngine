namespace PrintEngine.Core.Interfaces
{
	public interface IPrintModel
	{
		void Validate();
		void SetValue(object value, string name);
		T GetValue<T>(string name);
	}
}
