using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrintEngine.Core;
using PrintEngine.Core.Interfaces;
using PrintEngine.Core.Models;
using PrintEngine.Core.Services;
using PrintEngine.Metadata.Infrastructure;
using PrintEngine.Metadata.Services;
using PrintEngine.Resources.Repositories;
using PrintEngine.Templates.Helpers;
using PrintEngine.Templates.Services;
using System.Collections.Immutable;
using System.Reflection;

namespace PrintEngine.Templates.Infrastructure
{
	public static class Bootstrapper
	{
		public static IServiceCollection RegisterPrintEngine(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = new PrintEngineSettings();
			configuration.Bind(nameof(PrintEngineSettings), settings);
			return services.RegisterPrintEngine(settings);
		}
		public static IServiceCollection RegisterPrintEngine(this IServiceCollection services, IPrintEngineSettings settings)
		{
			var templates = Assembly.GetAssembly(typeof(Bootstrapper))?
			.GetExportedTypes()?
			.Where(t => t.GetInterface(nameof(IPrintTemplate)) != null)
			.Select(t =>
			{
				var attr = t.GetCustomAttribute<TemplateAttribute>();
				if (attr == null)
					throw new PrintTemplateException($"Не указан Template атрибут для {t.Name}");

				return (attr, t);
			})
			.Where(((TemplateAttribute template, Type type) t) => t.template.Files.IsEmpty() == false)
			.SelectMany(((TemplateAttribute template, Type type) t) =>
			{
				return t.template.Templates.Zip(t.template.Files, (templateId, fileId) =>
				{
					PrintTemplateFactory.RegisterTemplate(templateId, t.type);
					return new KeyValuePair<string, string>(templateId, fileId);
				});
			})
			.ToArray();

			var templateRepository = new PrintTemplateRepository();
			templateRepository.Register(templates);

			Assembly.GetAssembly(typeof(Bootstrapper))?
				.GetExportedTypes()?
				.Where(t => t.GetInterface(nameof(IPrintMapper)) != null)
				.Select(t =>
				{
					var args = GetGenericArguments(t);
					return (args[0], args[1], t);
				})
				.ToImmutableList()
				.ForEach(((Type inputModel, Type printModel, Type type) mt)
					=> PrintMapperFactory.RegisterMapper(mt.inputModel, mt.printModel, mt.type));

			services
				.AddHttpClient()
				.AddSingleton(settings)
				.AddSingleton<IPrintTemplateRepository>(templateRepository)
				.AddSingleton<IResourceRepository, ResourceRepository>()
				.AddSingleton<Func<IHttpClientFactory, string, ISignService>>((factory, url) => new SignService(factory, url))
				.AddSingleton<Func<IHttpClientFactory, string, IQrCodeService>>((factory, url) => new QrCodeService(factory, url))
				.AddSingleton<IPrintTemplateFactory, PrintTemplateFactory>()
				.AddSingleton<IPrintMapperFactory, PrintMapperFactory>()
				.AddSingleton<IResourceService, ResourceService>()
				.AddSingleton<IPrintEngineContext, PrintEngineContext>()
				.AddTransient<IPrintComposer, DynamicPrintComposer>()
				;

			services.RegisterMetadataMapping(settings);

			return services;
		}
		private static Type[] GetGenericArguments(Type type)
		{
			var types = type.GenericTypeArguments;
			if (types.Length < 2)
				return GetGenericArguments(type.BaseType);
			return types;
		}
	}
}
