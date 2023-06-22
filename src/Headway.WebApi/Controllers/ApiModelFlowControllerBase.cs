using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    public abstract class ApiModelFlowControllerBase<TModel, TController, TFlowContext> : ApiModelControllerBase<TModel, TController>
    {
        protected readonly IRepositoryFlowable<TFlowContext> repositoryFlowable;

        public ApiModelFlowControllerBase(
            IRepositoryFlowable<TFlowContext> repositoryFlowable,
            ILogger<TController> logger)
            : base(repositoryFlowable, logger)
        {
            this.repositoryFlowable = repositoryFlowable;
        }

        [HttpPost("[action]")]
        public abstract Task<IActionResult> FlowExecution([FromBody] TModel model);

        protected bool IsFlowUserAuthenticatedAsync(IFlowContext flowContext)
        {
            var claim = GetUserClaim();

            return flowContext.Authorisation.User.Equals(claim);
        }
    }
}
