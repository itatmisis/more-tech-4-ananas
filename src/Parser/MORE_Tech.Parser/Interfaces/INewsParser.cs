using MORE_Tech.Data.Models;

namespace MORE_Tech.Parser.Interfaces
{
    public interface INewsParser
    {
        Task Parse(NewsSource source);
    }
}
