using PrintEngine.Controllers;
using PrintEngine.Extentions;
using PrintEngine.Templates.Helpers;
using PrintEngine.Templates.Infrastructure;
using Swashbuckle.AspNetCore.Filters;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services
			.RegisterPrintEngine(builder.Configuration)
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(options =>
			{
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, @"PrintEngineAPI.xml"));
				options.ExampleFilters();
				options.EnableAnnotations();
				options.DocumentFilter<DocumentFilter>();
			})
			.AddSwaggerExamplesFromAssemblyOf<PrintEngineController>()
			.AddControllers()
			.AddNewtonsoftJson(option =>
			{
				option.SerializerSettings.Culture = Utils.RuCulture;
			});

		var app = builder.Build();

		app.UseStaticFiles();
		app.MapControllers();
		// use swagger in developer mode
		// if (app.Environment.IsDevelopment())
		if (app.Configuration.IsTestEnvironment())
		{
			app.UseSwagger()
				.UseSwaggerUI(op =>
				{
					op.DefaultModelsExpandDepth(-1);
				});
		}

		app.Run();
	}
}