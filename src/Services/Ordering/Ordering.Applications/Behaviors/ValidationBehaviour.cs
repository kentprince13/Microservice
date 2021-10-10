using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = Ordering.Applications.Exceptions.ValidationException;

namespace Ordering.Applications.Behaviors
{
    public class ValidationBehaviour<TRequest,TResponse> :IPipelineBehavior<TRequest,TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validator)
        {
            _validator = validator;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResult =
                    await Task.WhenAll(_validator.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0) throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
 