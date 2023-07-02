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
            if(flowContext == null)
            {
                throw new ArgumentNullException(nameof(flowContext));
            }

            RedressFlowContext redressFlowContext;

            if(flowContext.RedressFlowContextId.Equals(0))
            {
                var authorisation = await remediatRRedressRepository.GetAuthorisationAsync(flowContext.Authorisation?.User)
                    .ConfigureAwait(false);

                redressFlowContext = new RedressFlowContext
                {
                    FlowId = flowContext.FlowId,
                    Flow = flowContext.Flow,
                    Authorisation = authorisation
                };
            }
            else
            {
                redressFlowContext = await remediatRRedressRepository
                    .GetFlowContextAsync(flowContext.RedressFlowContextId)
                    .ConfigureAwait(false);
            }

            await redressFlowContext
                .ExecuteAsync(flowContext.FlowExecutionArgs)
                .ConfigureAwait(false);

            return await remediatRRedressRepository
                .UpdateFlowHistoryAsync(redressFlowContext)
                .ConfigureAwait(false);
        }
    }
}
