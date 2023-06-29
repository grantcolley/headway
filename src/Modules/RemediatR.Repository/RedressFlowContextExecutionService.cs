using Headway.Core.Extensions;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System;
using System.Threading.Tasks;

namespace RemediatR.Repository
{
    public class RedressFlowContextExecutionService : IRedressFlowContextExecutionService
    {
        private readonly IRemediatRRepository<RedressFlowContext> remediatRRepository;

        public RedressFlowContextExecutionService(IRemediatRRepository<RedressFlowContext> remediatRRepository) 
        {
            this.remediatRRepository = remediatRRepository;
        }

        public async Task<RedressFlowContext> Execute(RedressFlowContext flowContext)
        {
            var currentFlowContext = await remediatRRepository
                 .GetFlowContextAsync(flowContext.RedressFlowContextId)
                 .ConfigureAwait(false);

            await currentFlowContext
                .ExecuteAsync(flowContext.FlowExecutionArgs, flowContext.Authorisation)
                .ConfigureAwait(false);

            return currentFlowContext;
        }
    }
}
