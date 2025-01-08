using System.ComponentModel;

namespace PrintEngine.Core.Models
{
    public class PrintResult : Result
    {
        protected PrintResult() { }
       
        [Description("Идентификатор")]
        public Guid? CorrelationId { get; protected set; }
        
        [Description("Описание файла печтаной формы")]
        public PrintData PrintData { get; protected set; }
        
        [Description("Описание файла ЭЦП (возможно)")]
        public SignData SignData { get; protected set; }

        public static PrintResult Success(PrintData printData, SignData signData = null, Guid? correlationId = null)
        {
            return new PrintResult()
            {
                PrintData = printData,
                SignData = signData,
                IsSuccess = true,
                CorrelationId = correlationId
            };
        }
        public static PrintResult Fail(string error, Guid? correlationId = null)
        {
            return new PrintResult() { Error = error, IsSuccess = false, CorrelationId = correlationId };
        }

        public override string ToString()
        {
            return $"IsSuccess = {IsSuccess}, CorrelationId = {CorrelationId}, FileName = \"{PrintData?.FileName}\", PageCount = {PrintData?.PageCount}, OutputLength = {PrintData?.OutputLength}";
        }
    }
}
