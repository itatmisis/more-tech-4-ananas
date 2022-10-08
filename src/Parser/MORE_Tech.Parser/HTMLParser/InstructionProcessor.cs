
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Options;
using MORE_Tech.Parser.Configuration;
using MORE_Tech.Parser.HTMLParser.Models;

namespace MORE_Tech.Parser.HTMLParser
{
    public class InstructionProcessor
    {
        private readonly string _pathToXmlFiles;
        private readonly ILogger<InstructionProcessor> _logger;
        public InstructionProcessor(IOptions<AppSettings> options, ILogger<InstructionProcessor> logger)
        {
            _pathToXmlFiles = options.Value.PathToXmlFiles;
            _logger = logger;
        }
        public HtmlParseInstructions getInstructions(int sourceId)
        {
            var doc = getXml(sourceId);
            return parseDoc(doc);
        }

        private HtmlParseInstructions parseDoc(XmlDocument doc)
        {
            HtmlParseInstructions instructions = new HtmlParseInstructions();
            var instructionsElement = doc.DocumentElement;

            if(instructionsElement is null)
            {
                throw new Exception("Invalid xml document");
            }
            foreach (XmlElement instruction in instructionsElement)
            {
                if(instruction?.FirstChild?.InnerText == null)
                {
                    throw new Exception($"Instruction with name: {instruction?.Name} doesn");
                }
                switch (instruction?.Name)
                {
                    case nameof(HtmlParseInstructions.NewsName):
                        instructions.NewsName = new()
                        {
                            Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                            AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value,
                            Regex = instruction.Attributes?.GetNamedItem("regex")?.Value
                        };
                        break;
                    case nameof(HtmlParseInstructions.NewsText):
                        instructions.NewsText = new()
                        {
                            Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                            AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value,
                            Regex = instruction.Attributes?.GetNamedItem("regex")?.Value
                        };
                        break;
                    case nameof(HtmlParseInstructions.Views):
                        instructions.Views = new()
                        {
                            Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                            AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value,
                            Regex = instruction.Attributes?.GetNamedItem("regex")?.Value
                        };
                        break;
                    case nameof(HtmlParseInstructions.DateTime):
                        instructions.DateTime = new()
                        {
                            Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                            AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value,
                            Regex = instruction.Attributes?.GetNamedItem("regex")?.Value
                        };
                        break;
                    case nameof(HtmlParseInstructions.Images):
                        instructions.Images = new()
                        {
                            Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                            AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value,
                            Regex = instruction.Attributes?.GetNamedItem("regex")?.Value
                        };
                        break;
                    case nameof(HtmlParseInstructions.RootUrl):
                        instructions.RootUrl = instruction.FirstChild.InnerText;
                        break;

                    case nameof(HtmlParseInstructions.FeedUrls):
                        foreach (XmlElement url in instruction.ChildNodes)
                        {
                            if(url?.FirstChild?.InnerText == null)
                            {
                                throw new Exception($"Instruction for {nameof(HtmlParseInstructions.FeedUrls)} is invalid");
                            }
                            instructions.FeedUrls.Add(new Regex($@"{url.FirstChild.InnerText}"));
                        }
                        break;
                    case nameof(HtmlParseInstructions.NewsUrls):
                        foreach (XmlElement url in instruction.ChildNodes)
                        {
                            if (url?.FirstChild?.InnerText == null)
                            {
                                throw new Exception($"Instruction for {nameof(HtmlParseInstructions.NewsUrls)} is invalid");
                            }
                            instructions.NewsUrls.Add(new Regex($@"{url.FirstChild.InnerText}"));
                        }
                        break;
                }
            }

            return instructions;
        }

        private XmlDocument getXml(int sourceId)
        {
            if (!File.Exists(Path.Combine(_pathToXmlFiles, $"{sourceId}.xml")))
            {
                throw new Exception($"File: {sourceId}.xml not found");
            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Path.Combine(_pathToXmlFiles, $"{sourceId}.xml"));
                return xDoc;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while opening file: {sourceId}.xml. Meesage: {ex.Message}");
            }

        }
    }
}
