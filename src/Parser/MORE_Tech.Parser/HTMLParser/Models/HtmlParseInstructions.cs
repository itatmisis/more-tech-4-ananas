using System.Text.RegularExpressions;

namespace MORE_Tech.Parser.HTMLParser.Models
{
    public class HtmlParseInstructions
    {

        /// <summary>
        /// Ссылки на летну новостей
        /// </summary>
        public List<Regex> FeedUrls { get; set; }

        /// <summary>
        /// Ссылки на страницу с новостью
        /// </summary>
        public List<Regex> NewsUrls { get; set; }
        public string RootUrl { get; set; }
        public NewsItemInstruction NewsName { get; set; }
        public NewsItemInstruction NewsText { get; set; }
        public NewsItemInstruction Views { get; set; }
        public NewsItemInstruction DateTime { get; set; }

        public HtmlParseInstructions()
        {
            NewsUrls = new();
            FeedUrls = new();
        }
    }
}
