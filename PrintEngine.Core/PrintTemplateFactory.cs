using PrintEngine.Core.Interfaces;
using System.Collections.Concurrent;

namespace PrintEngine.Core
{
	public class PrintTemplateFactory : IPrintTemplateFactory
	{
		private static ConcurrentDictionary<string, TemplateFactoryParams> _storageTemplate = new();
		private readonly IPrintEngineContext _engineContext;

		public PrintTemplateFactory(IPrintEngineContext engineContext)
		{
			_engineContext = engineContext ?? throw new ArgumentNullException(nameof(engineContext));
		}
		public IPrintTemplate GetTemplate(string templateId)
		{
			try
			{
				return _storageTemplate.GetOrAdd(templateId, (id) => null)
						?.Invoke(_engineContext);
			}
			catch
			{
				return null;
			}
		}
		public static void RegisterTemplate(string Id, Type type)
		{
			_storageTemplate.TryAdd(Id, new TemplateFactoryParams
			{
				TemplateType = type,
				TemplateId = Id
			});
		}
		class TemplateFactoryParams
		{
			public string TemplateId;
			public Type TemplateType;
			public IPrintTemplate Invoke(IPrintEngineContext context)
			{
				var template = (IPrintTemplate)Activator.CreateInstance(TemplateType);
				template?.SetTemplateId(TemplateId);
				template?.SetContext(context);
				return template;
			}
		}
	}
}
