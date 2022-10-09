using System.Xml.XPath;

namespace MORE_Tech.Parser.HTMLParser.Models
{
    public class NewsItemInstruction
    {
        public XPathExpression Expression { get; set; }
        public  string? AttributeName { get; set; }
        public string? Regex { get; set; }
    }
}
