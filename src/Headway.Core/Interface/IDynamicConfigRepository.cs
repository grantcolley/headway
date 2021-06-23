using Headway.Core.Dynamic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicConfigRepository : IRepository
    {
        Task<DynamicModelConfig> GetDynamicModelConfigAsync(string model);
    }
}
