using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepository
    {
        Task<bool> IsAuthorisedAsync(string userName, string permission);
    }
}
