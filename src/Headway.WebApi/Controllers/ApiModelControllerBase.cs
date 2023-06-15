using Headway.Core.Args;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Headway.WebApi.Controllers
{
    public abstract class ApiModelControllerBase<TModel,TController> : ApiControllerBase<TController>
    {
        public ApiModelControllerBase(
            IRepository repository,
            ILogger<TController> logger)
            : base(repository, logger)
        {
        }

        [HttpGet]
        public abstract Task<IActionResult> Get();

        [HttpGet]
        public abstract Task<IActionResult> Get(int id);

        [HttpPost]
        public abstract Task<IActionResult> Post([FromBody] TModel model);

        [HttpPut]
        public abstract Task<IActionResult> Put([FromBody] TModel model);

        [HttpDelete]
        public abstract Task<IActionResult> Delete(int id);

        [HttpPost("[action]")]
        public virtual Task<IActionResult> Search([FromBody] SearchArgs searchArgs)
        {
            throw new NotImplementedException(searchArgs.SourceConfig);
        }
    }
}
