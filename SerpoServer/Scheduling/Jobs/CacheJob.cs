using System.Threading.Tasks;
using Quartz;

namespace SerpoServer.Scheduling.Jobs
{
    public class CacheJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}