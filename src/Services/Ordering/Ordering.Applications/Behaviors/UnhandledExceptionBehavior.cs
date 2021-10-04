using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Applications.Behaviors
{
    public class UnhandledExceptionBehavior<T,R>:IPipelineBehavior<T,R>
    {
        private readonly ILogger<T> _logger;

        public UnhandledExceptionBehavior(ILogger<T> logger)
        {
            _logger = logger;
        }
        public async Task<R> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            try
            {
              return  await next();
            }
            catch (Exception e)
            {
                var requestName = typeof(T).Name;
                _logger.LogError(e, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
                throw;

            }
        }
    }
}
