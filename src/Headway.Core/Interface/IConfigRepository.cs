using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigRepository : IRepository
    {
        Task<ModelConfig> GetModelConfigAsync(string model);
    }
}
