using Headway.Core.Model;

namespace Headway.Core.Interface
{
    public interface ILogRepository : IRepository
    {
        void LogAsync(Log log);
    }
}
