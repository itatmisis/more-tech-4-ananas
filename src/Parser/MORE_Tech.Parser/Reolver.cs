using MORE_Tech.Data.Models.Enums;
using MORE_Tech.Parser.Interfaces;

namespace MORE_Tech.Parser
{
    public delegate INewsParser ParserResolver(SourceTypes type);

}
