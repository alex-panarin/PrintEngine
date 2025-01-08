using PrintEngine.Core.Models;

namespace PrintEngine.Core.Interfaces
{
	/// <summary>
	/// Вызов сервиса цифровой подписи
	/// </summary>
	public interface ISignService : IDisposable
	{
		Task<byte[]> SignDocument(SignRequest request);
	}
}
