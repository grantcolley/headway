using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface ILogApiRequest
    {
        Task LogAsync(Log log);
    }
}
