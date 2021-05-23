using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IRepository
    {
        Task<bool> IsAuthorisedAsync(string claim, string permission);
    }
}
