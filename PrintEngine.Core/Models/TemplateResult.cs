using System.ComponentModel;

namespace PrintEngine.Core.Models
{
	public class TemplateResult : Result
	{
		protected TemplateResult() { }
        
        [Description("Список реализованных ПФ")]
        public KeyValuePair<string, string>[] Templates { get; protected set; }
		public static TemplateResult Success(KeyValuePair<string, string>[] keyValuePairs)
		{
			return new TemplateResult()
			{
				IsSuccess = true,
				Templates = keyValuePairs	
			};
		}
		public static TemplateResult Fail(string error)
		{
			return new TemplateResult() { Error = error, IsSuccess = false };
		}
	}
}
