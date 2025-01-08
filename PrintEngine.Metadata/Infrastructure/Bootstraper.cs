using Microsoft.Extensions.DependencyInjection;
using PrintEngine.Core.Models;
using PrintEngine.Metadata.Interfaces;
using PrintEngine.Metadata.Services;
using PrintEngine.Metadata.Services.Local;

namespace PrintEngine.Metadata.Infrastructure
{
	public static class Bootstraper
	{
		public static IServiceCollection RegisterMetadataMapping(this IServiceCollection services, IPrintEngineSettings settings)
		{

			if (settings.UseLocalMetadata == true)
			{
				services
					.AddSingleton<IMetadataService, MetadataService>()
					.AddSingleton<IMetadataRepository, LocalMetadataRepository>()
					.AddSingleton<IMetadataProvider, LocalMetadataProvider>();
			}
			else
			{
				//services
				//    .RegisterMetadataClient(settings.MetadataServiceUrl)
				//    .AddScoped<IMetadataService, MetadataService>()
				//    .AddScoped<IMetadataRepository, RemoteMetadataRepository>();
			}
			return services;
		}
	}
}
