using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigRepository : IRepository
    {
        Task<ListConfig> GetListConfigAsync(string model);
        Task<ModelConfig> GetModelConfigAsync(string model);
    }
}
