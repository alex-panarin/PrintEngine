using System.ComponentModel;

namespace PrintEngine.Core.Models
{
	public class Result
	{
		protected Result() { }
		
		[Description("Успешность выполнения операции")]
		public bool IsSuccess { get; protected set; }

        [Description("Описание ошибки")]
        public string Error { get; protected set; }
	}
}
