using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using MORE_Tech.Data;
using MORE_Tech.Data.Models;
using MORE_Tech.Data.Models.Enums;
using MORE_Tech.Parser.Configuration;
using MORE_Tech.Parser.HTMLParser;
using MORE_Tech.Parser.HTMLParser.Models;
using MORE_Tech.Parser.Interfaces;
using MORE_Tech.Parser.RequetsHelper;
using System.Text;
using System.Text.RegularExpressions;

namespace MORE_Tech.Parser.ParserImplementations
{
    public class HtmlParser : INewsParser
    {

        private readonly InstructionProcessor _instrutionProcessor;
        private  HtmlParseInstructions _parseInstructions;
        private readonly List<string> _visitedUrls;
        private readonly IUnitOfWork _unitOfWork;
        private NewsSource _source;
        private readonly ILogger<HtmlParser> _logger;

        private readonly Object _locker = new object();
        private int recutsionDepth = 0;

        public HtmlParser(IOptions<AppSettings> settings, IUnitOfWork unitOfWork, ILogger<HtmlParser> logger)
        {
            _instrutionProcessor = new InstructionProcessor(settings.Value);

            _unitOfWork = unitOfWork ?? 
                throw new ArgumentNullException(nameof(unitOfWork));

            _visitedUrls = new List<string>();

            _logger = logger;
        }
        public async Task Parse(NewsSource source)
        {
            _logger.LogInformation($"Start parsing source: {source.Id}");
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            _source = source;

            _parseInstructions = _instrutionProcessor.getInstructions(source.Id) ??
                throw new Exception("Instructions not found");

            await ParseUrl(source.Url);

            _logger.LogInformation($"End parsing source: {source.Id}");
        }

        private async Task ParseUrl(string url)
        {
            if (!url.Contains(_parseInstructions.RootUrl))
            {
                url = _parseInstructions.RootUrl + url;
            }
            if (IsFeed(url))
            {
                try
                {
                    await parseFeed(url);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error while parsing Feed: {url}. Error: {ex.Message}");
                }
            }
            else if (IsNews(url))
            {
                try
                {
                    await parseNews(url);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error while parsing news: {url}. Error: {ex.Message}");
                }
            }
        }

        private async Task parseFeed(string url)
        {
            _logger.LogInformation($"Start parsing FeedUrl: {url}");
            if (_visitedUrls.Contains(url))
            {
                throw new Exception($"Url {url} already visited");
            }
            _visitedUrls.Add(url);
            if (!url.Contains(_parseInstructions.RootUrl))
            {
                url = _parseInstructions.RootUrl + url;
            }

            if (recutsionDepth > 1000)
            {
                throw new Exception("Recursion level more than 1000");
            }

            recutsionDepth++;
           

            try
            {
                string body = await HttpRequests.Send(new Uri(url));

                if (string.IsNullOrEmpty(body))
                {
                    return;
                }

                List<string> newsUrls = getUrlsNews(body);

                var parseTasks = new List<Task<News>>();

                parseTasks.AddRange(newsUrls.Select(
                  url => parseNews(url)));


                var parsedNews = await Task.WhenAll(parseTasks);

                    foreach(var news in parsedNews.Where(n => n!=null))
                {
                    try
                    {
                        await SaveNews(news);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error while saving news: {ex}");
                    }
                }
                

                List<string> feedUrls = getUrlsFeed(body);

                foreach(var feedUrl in feedUrls)
                {
                    try
                    {
                        await parseFeed(feedUrl);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error while parsing feed: {ex.Message}");
                    }
                }
                recutsionDepth--;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing url:{url}. Error: {ex}");
                recutsionDepth--;
                return;
            }

           
        }

        private async Task<News> parseNews(string url)
        {
            lock (_locker)
            {
                if (_visitedUrls.Contains(url))
                {
                    _logger.LogWarning($"Url: {url} already vidited");
                    throw new Exception("Url already visited");
                }
                _visitedUrls.Add(url);
            }
            _logger.LogInformation($"Start parsing news: {url} NNNN:{recutsionDepth}");
            if (!url.Contains(_parseInstructions.RootUrl))
            {
                url = _parseInstructions.RootUrl + url;
            }
            if (_visitedUrls.Contains(url))
            {
                throw new Exception($"Url: {url} already visited");
            }
            
            if(recutsionDepth > 100)
            {
                throw new Exception("Recursion level more than 10");
            }
           
            string body = string.Empty;
            try
            {
                body = await HttpRequests.Send(new Uri(url));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while sending httpReuest: {url}");
                throw;
            }

            try
            {
                var news = ParseNews(body, url);
                _logger.LogInformation($"End parsing url:{url}");
                return news;
            }
             catch(Exception ex)
            {
                _logger.LogError($"Error while parsing blocks of news: {url}");
                throw;
            }
        }

        private async Task SaveNews(News news)
        {
            await _unitOfWork.NewsRepository.AddAsync(news);
            await _unitOfWork.SaveChanges();
            _logger.LogInformation($"News {news} saved");
        }

        private News? ParseNews(string body, string url)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(body);

            string text = parseItem(doc, _parseInstructions.NewsText);

            if( text == null)
            {
                _logger.LogError($"Error while parsing text of news: {url}");
                return null;
            }

            int view;
            if(_parseInstructions.Views != null)
            {
                if (!int.TryParse(parseItem(doc, _parseInstructions.Views), out view))
                {
                    _logger.LogError($"Error while parsing views of news: {url}");
                    return null;
                }
            }
            else
            {
                view = 0;
            }
            

            var dateString = parseItem(doc, _parseInstructions.DateTime);
            if (!DateTime.TryParse(dateString, out DateTime date))
            {
                _logger.LogError($"Error while parsing date of news: {url}");
                return null;
            }
            var news = new News(string.Empty, text, url, view, date, _source.Id);
            if (_parseInstructions.Images != null)
            {
                var images = parseAttechesItems(doc, _parseInstructions.Images);
                if (images != null && images.Any())
                {
                    images
                       .ForEach(x => {
                           news.Attachments.Add(new Attachments(x, AttachmentTypes.Photo));
                       });
                }
            }
         


            return news;
        }


        private string parseItem(HtmlDocument doc, NewsItemInstruction itemInstruction)
        {
            var elem = doc.DocumentNode.SelectNodes(itemInstruction.Expression);

            string result = string.Empty;
            if (elem != null && elem.Any())
            {

                if (!string.IsNullOrEmpty(itemInstruction.AttributeName))
                {
                    var elemWithValue = elem.FirstOrDefault(x => !string.IsNullOrEmpty(x.Attributes[itemInstruction.AttributeName]?.Value));
                    if(elemWithValue != null)
                    {
                        var attrValue = elemWithValue.Attributes[itemInstruction.AttributeName].Value;
                        if (!string.IsNullOrEmpty(itemInstruction.Regex))
                        {
                            var match = new Regex(itemInstruction.Regex).Matches(attrValue);
                            return  match?.FirstOrDefault().Value;
                        }
                        return attrValue;
                    }
                    return null;
                }
                else
                {
                    if (!string.IsNullOrEmpty(itemInstruction.Regex))
                    {
                        var match = new Regex(itemInstruction.Regex).Matches(elem.First().InnerText);
                        return match?.FirstOrDefault().Value;
                    }
                    result = elem.First().InnerText;
                }
            }
            return result;
        }


        private List<string> parseAttechesItems(HtmlDocument doc, NewsItemInstruction itemInstruction)
        {
            var elem = doc.DocumentNode.SelectNodes(itemInstruction.Expression);

            List<string> results = new();
            if (elem != null && elem.Any())
            {

                if (!string.IsNullOrEmpty(itemInstruction.AttributeName))
                {
                    var elemsWithValue = elem.Where(x => !string.IsNullOrEmpty(x.Attributes[itemInstruction.AttributeName]?.Value));
                    foreach(var elemVal in elemsWithValue)
                    {
                        if (elemVal != null)
                        {
                            var attrValue = elemVal.Attributes[itemInstruction.AttributeName].Value;
                            if (!string.IsNullOrEmpty(itemInstruction.Regex))
                            {
                                var match = new Regex(itemInstruction.Regex).Matches(attrValue);
                                results.Add(match?.FirstOrDefault().Value);
                            }
                            results.Add(attrValue);
                        }
                    }
                }
                else
                {
                    results = elem.Select(x => x.InnerText)
                        .Where(x => x!= null)
                        .ToList();
                }
            }
            return results;
        }

        private List<string> getUrlsNews(string body)
        {
            return getUrls(_parseInstructions.NewsUrls, body);
        }

        private List<string> getUrlsFeed(string body)
        {
            return getUrls(_parseInstructions.FeedUrls, body);
        }

        private List<string> getUrls(IEnumerable<Regex> expressions, string body)
        {
            var urls = new List<string>();
            foreach (var regxListProd in expressions)
            {
                var matches = regxListProd.Matches(body);
                urls.AddRange(matches.Select(x => x.Value.Trim()));
            }

            return urls.Distinct()
                .Where(url => !_visitedUrls.Contains(url))
                .ToList();

        }

        private bool IsFeed(string url)
        {
            return _parseInstructions.FeedUrls.Any(x => x.IsMatch(url));
        }

        private bool IsNews(string url)
        {
            return _parseInstructions.NewsUrls.Any(x =>x.IsMatch(url));
        }





    }
}
