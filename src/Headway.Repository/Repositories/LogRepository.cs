using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.Extensions.Logging;

namespace Headway.Repository.Repositories
{
    public class LogRepository : RepositoryBase<LogRepository>, ILogRepository
    {
        public LogRepository(ApplicationDbContext applicationDbContext, ILogger<LogRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public void LogAsync(Log log)
        {
            if(log == null)
            {
                return;
            }

            switch(log.Level)
            {
                case Core.Enums.LogLevel.Information:
                    logger.LogInformation(log.Message);
                    break;
                case Core.Enums.LogLevel.Warning:
                    logger.LogWarning(log.Message);
                    break;
                case Core.Enums.LogLevel.Error:
                    if(log.Exception != null)
                    {
                        logger.LogError(log.Exception, log.Message);
                    }
                    else
                    {
                        logger.LogError(log.Message);
                    }

                    break;
            }
        }
    }
}
