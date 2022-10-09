using Microsoft.Extensions.Options;
using MORE_Tech.Data;
using MORE_Tech.Data.Models;
using MORE_Tech.Data.Models.Enums;
using MORE_Tech.Parser.Configuration;
using MORE_Tech.Parser.Interfaces;
using MORE_Tech.Parser.ParserImplementations;
using System.Diagnostics;

namespace MORE_Tech.Parser.Service
{
    public class ParserWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly VKSettings _settings;
        private readonly ILogger<ParserWorker> _logger;
        public ParserWorker(IServiceScopeFactory serviceScopeFactory, IOptions<VKSettings> settings,
              ILogger<ParserWorker> logger)
        {
            _settings = settings?.Value ??
                throw new ArgumentNullException(nameof(settings));
            _serviceScopeFactory = serviceScopeFactory ??
                throw new ArgumentNullException(nameof(serviceScopeFactory));
    
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                await runParser(stoppingToken);
                sw.Stop();
                
                await Task.Delay(1000 * 60 * 60 * 24 - (int)sw.ElapsedMilliseconds);
            }
        }

        private List<NewsSource> getSources()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                return unitOfWork.NewsSourceRespository.GetAllActive()
                    .ToList();
                
            }
        }

        private async Task runParser(CancellationToken stoppingToken)
        {
            foreach (var source in getSources())
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("App is stopped");
                    return;
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    INewsParser parser;
                    try
                    {
                        if (source.Type == SourceTypes.HTML)
                            parser = scope.ServiceProvider.GetService<HtmlParser>();
                        else
                            parser = scope.ServiceProvider.GetService<VKParser>();

                        if (parser == null)
                        {
                            _logger.LogError($"Parser of type: {source.Type} resolved to null");
                            continue;
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while resolving parser of type: {source.Type}. Message:{ex.Message}");
                        continue;
                    }
                    try
                    {
                        await parser.Parse(source);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error while HTML parser working: {ex.Message}");
                    }
                }

            }
        }
    }
}
