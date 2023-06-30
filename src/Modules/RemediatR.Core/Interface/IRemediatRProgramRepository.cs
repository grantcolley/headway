using Headway.Core.Interface;
using RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemediatR.Core.Interface
{
    public interface IRemediatRProgramRepository : IRepository
    {
        Task<IEnumerable<Program>> GetProgramsAsync();
        Task<Program> GetProgramAsync(int id);
        Task<Program> AddProgramAsync(Program program);
        Task<Program> UpdateProgramAsync(Program program);
        Task<int> DeleteProgramAsync(int id);
    }
}
