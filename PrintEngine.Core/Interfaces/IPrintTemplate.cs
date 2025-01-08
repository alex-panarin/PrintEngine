using PrintEngine.Core.Models;

namespace PrintEngine.Core.Interfaces
{
	public interface IPrintTemplate
        : IDisposable
    {
        Type GetModelType();
        Task<(PrintData, SignData)> GetDocument(IPrintModel model, bool needSign, bool printHeader = false, string correlationId = null);
        Type[] GetValidInputDataTypes();
        void SetContext(IPrintEngineContext context);
        void SetTemplateId(string tempalteId);
	}
}
