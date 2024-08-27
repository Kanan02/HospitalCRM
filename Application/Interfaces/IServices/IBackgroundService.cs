using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IBackgroundService
    {
        string Schedule(Expression<Action> methodCall, DateTime executionDateTime);
    }
}
