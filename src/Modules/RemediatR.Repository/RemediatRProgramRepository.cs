using Headway.Repository.Data;
using Headway.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemediatR.Repository
{
    public class RemediatRProgramRepository : RepositoryBase<RemediatRProgramRepository>, IRemediatRProgramRepository
    {
        public RemediatRProgramRepository(ApplicationDbContext applicationDbContext, ILogger<RemediatRProgramRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Program>> GetProgramsAsync()
        {
            return await applicationDbContext.Programs
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Program> GetProgramAsync(int id)
        {
            return await applicationDbContext.Programs
                .AsNoTracking()
                .FirstAsync(p => p.ProgramId.Equals(id))
                .ConfigureAwait(false);
        }

        public async Task<Program> AddProgramAsync(Program program)
        {
            await applicationDbContext.Programs
                .AddAsync(program)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return program;
        }

        public async Task<Program> UpdateProgramAsync(Program program)
        {
            applicationDbContext.Programs.Update(program);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return program;
        }

        public async Task<int> DeleteProgramAsync(int id)
        {
            var program = await applicationDbContext.Programs
                .FirstAsync(p => p.ProgramId.Equals(id))
                .ConfigureAwait(false);

            applicationDbContext.Programs.Remove(program);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
