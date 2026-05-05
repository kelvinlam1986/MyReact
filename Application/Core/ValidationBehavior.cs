using FluentValidation;
using MediatR;

namespace Application.Core
{
    public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator = null)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validator == null)
            {
                return await next();
            }

            var validateResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            return await next();
        }
    }
}
