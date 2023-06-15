using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IFlowContextExecutionService<T>
    {
        Task<T> Execute(T flowContext);
    }
}
