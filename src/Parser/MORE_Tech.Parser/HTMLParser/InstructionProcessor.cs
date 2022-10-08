
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
        public InstructionProcessor(AppSettings options)
        {
            _pathToXmlFiles = options.PathToXmlFiles;
        }
        public HtmlParseInstructions getInstructions(int sourceId)
        {
            var doc = getXml(sourceId);
            if (doc is null)
            {
                return null;
            }

            return parseDoc(doc);
        }

        private HtmlParseInstructions parseDoc(XmlDocument doc)
        {
            HtmlParseInstructions instructions = new HtmlParseInstructions();
            try
            {
                var instructionsElement = doc.DocumentElement;

                if(instructionsElement is null)
                {
                    throw new Exception("Invalid xml document");
                }
                foreach (XmlElement instruction in instructionsElement)
                {
                    if(instruction?.FirstChild?.InnerText == null)
                    {
                        throw new Exception("Invalid xml instruction");
                    }
                    switch (instruction?.Name)
                    {
                        case nameof(HtmlParseInstructions.NewsName):
                            instructions.NewsName = new()
                            {
                                Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                                AttributeName = instruction.Attributes?.GetNamedItem("takenAttrubute")?.Value
                            };
                            break;
                        case nameof(HtmlParseInstructions.NewsText):
                            instructions.NewsText = new()
                            {
                                Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                                AttributeName = instruction.Attributes?.GetNamedItem("takenAttrubute")?.Value
                            };
                            break;
                        case nameof(HtmlParseInstructions.Views):
                            instructions.Views = new()
                            {
                                Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                                AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value
                            };
                            break;
                        case nameof(HtmlParseInstructions.DateTime):
                            instructions.DateTime = new()
                            {
                                Expression = XPathExpression.Compile(instruction.FirstChild.InnerText),
                                AttributeName = instruction.Attributes?.GetNamedItem("attribute")?.Value
                            };
                            break;
                        case nameof(HtmlParseInstructions.RootUrl):
                            instructions.RootUrl = instruction.FirstChild.InnerText;
                            break;
                        case nameof(HtmlParseInstructions.FeedUrls):
                            foreach (XmlElement url in instruction.ChildNodes)
                            {
                                instructions.FeedUrls.Add(new Regex($@"{url.FirstChild.InnerText}"));
                            }
                            break;
                        case nameof(HtmlParseInstructions.NewsUrls):
                            foreach (XmlElement url in instruction.ChildNodes)
                            {
                                instructions.NewsUrls.Add(new Regex($@"{url.FirstChild.InnerText}"));
                            }
                            break;

                    }
                }

                return instructions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private XmlDocument getXml(int sourceId)
        {
            if (!File.Exists(Path.Combine(_pathToXmlFiles, $"{sourceId}.xml")))
            {
                return null;
            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Path.Combine(_pathToXmlFiles, $"{sourceId}.xml"));
                return xDoc;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}