using Microsoft.Extensions.Options;
using MORE_Tech.Data;
using MORE_Tech.Data.Models;
using MORE_Tech.Data.Models.Enums;
using MORE_Tech.Parser.Configuration;
using MORE_Tech.Parser.Interfaces;
using MORE_Tech.Parser.ParserImplementations;

namespace MORE_Tech.Parser.Service
{
    public class ParserWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly VKSettings _settings;
        public ParserWorker(IServiceScopeFactory serviceScopeFactory, IOptions<VKSettings> settings)
        {
            _settings = settings?.Value ??
                throw new ArgumentNullException(nameof(settings));
            _serviceScopeFactory = serviceScopeFactory ??
                throw new ArgumentNullException(nameof(serviceScopeFactory));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           foreach(var source in getSources())
           {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var parser = scope.ServiceProvider.GetService<INewsParser>();
                    await parser.Parse(source);
                }
                 
           }
        }

        private List<NewsSource> getSources()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
             {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                return unitOfWork.NewsSourceRespository.GetAllActive()
                    .Where(x => x.Type == SourceTypes.HTML).ToList();
              //  return unitOfWork.NewsSourceRespository.GetAllActive().Where(x => x.Type == Data.Models.Enums.SourceTypes.VK).ToList();

            }
        }
    }
}
