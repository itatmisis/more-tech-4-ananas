using MORE_Tech.Parser.Configuration;
using MORE_Tech.Data;
using MORE_Tech.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using MORE_Tech.Parser.Service;
using MORE_Tech.Parser.Interfaces;
using MORE_Tech.Data.Models.Enums;
using MORE_Tech.Parser.ParserImplementations;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace MORE_Tech.Parser
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            var appSettings = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);

            var vkSettings = _configuration.GetSection("VkSettings");
            services.Configure<VKSettings>(vkSettings);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IConfiguration config = builder.Build();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsSourceRespository, NewsSourceRepository>();
            services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();
            services.AddScoped<INewsParser,HtmlParser>();
            services.AddScoped<INewsParser, VKParser>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHostedService<ParserWorker>();

            services.AddLogging(opt =>
            {
                opt.AddConsole(c =>
                {
                    c.TimestampFormat = "[HH:mm:ss] ";
                });
            });



            services.AddDbContext<NewsDbContext>(options => options
            .UseNpgsql(appSettings.Get<AppSettings>().DbConnection, x => x.MigrationsAssembly("MORE_Tech.Data"))
            .UseLowerCaseNamingConvention());
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddTransient<ParserResolver>(serviceProvider =>
            {
                ParserResolver parserResolver = type =>
                            {
                                switch (type)
                                {
                                    case SourceTypes.VK:
                                        return serviceProvider.GetService<VKParser>();
                                    case SourceTypes.HTML:
                                        return serviceProvider.GetService<HtmlParser>();
                                    default:
                                        throw new Exception("Instructions not found");
                                }
                            };

                return parserResolver;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
        }
    }
}
