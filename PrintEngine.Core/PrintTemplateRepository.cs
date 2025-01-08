using PrintEngine.Core.Interfaces;
using System.Collections.Concurrent;

namespace PrintEngine.Core
{
	public class PrintTemplateRepository
		: IPrintTemplateRepository
	{
		private ConcurrentDictionary<string, string> _storageFile;

		public string GetTemplateFileName(string templateId)
		{
			return _storageFile?.GetOrAdd(templateId, v => null);
		}
		public KeyValuePair<string, string>[] GetTemplateNames()
		{
			return _storageFile?
				.ToArray()
				.Select(k => new KeyValuePair<string, string>(k.Value, k.Key))
				.ToArray();
		}
		public void Register(KeyValuePair<string, string>[] values)
		{
			_storageFile = new(values);
		}
	}
}
