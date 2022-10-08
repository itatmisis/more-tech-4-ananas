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
        private readonly ParserResolver _resolver;
        public ParserWorker(IServiceScopeFactory serviceScopeFactory, IOptions<VKSettings> settings, ParserResolver resolver)
        {
            _settings = settings?.Value ??
                throw new ArgumentNullException(nameof(settings));
            _serviceScopeFactory = serviceScopeFactory ??
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            _resolver = resolver ??
                throw new ArgumentNullException(nameof(resolver));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

           foreach(var source in getSources())
           {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var parser = _resolver(source.Type);
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
                return unitOfWork.NewsSourceRespository.GetAllActive().Where(x => x.Type == Data.Models.Enums.SourceTypes.VK).ToList();

            }
        }
    }
}
