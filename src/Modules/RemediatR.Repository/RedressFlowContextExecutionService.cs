using Headway.Core.Extensions;
using RemediatR.Core.Interface;
using RemediatR.Core.Model;
using System;
using System.Threading.Tasks;

namespace RemediatR.Repository
{
    public class RedressFlowContextExecutionService : IRedressFlowContextExecutionService
    {
        private readonly IRemediatRRedressRepository<RedressFlowContext> remediatRRedressRepository;

        public RedressFlowContextExecutionService(IRemediatRRedressRepository<RedressFlowContext> remediatRRedressRepository) 
        {
            this.remediatRRedressRepository = remediatRRedressRepository;
        }

        public async Task<RedressFlowContext> Execute(RedressFlowContext flowContext)
        {
            var currentFlowContext = await remediatRRedressRepository
                 .GetFlowContextAsync(flowContext.RedressFlowContextId)
                 .ConfigureAwait(false);

            await currentFlowContext
                .ExecuteAsync(flowContext.FlowExecutionArgs)
                .ConfigureAwait(false);

            return await remediatRRedressRepository
                .UpdateFlowContextAsync(currentFlowContext)
                .ConfigureAwait(false);
        }
    }
}
