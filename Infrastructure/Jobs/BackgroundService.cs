using Application.Interfaces.IServices;
using Hangfire;
using System.Linq.Expressions;

namespace Infrastructure.Jobs
{
    public class BackgroundService : IBackgroundService
    {
        public string Schedule(Expression<Action> methodCall,DateTime executionDateTime)
        {
            return BackgroundJob.Schedule(methodCall, executionDateTime);
        }
    }
}
