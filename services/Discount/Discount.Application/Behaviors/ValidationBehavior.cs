using FluentResults;
using FluentValidation;
using MediatR;

namespace Discount.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var errors = failures.Select(f => new Error(f.ErrorMessage)).ToList();

        return CreateFailedResult(errors);
    }

    private static TResponse CreateFailedResult(List<Error> errors)
    {
        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            return (TResponse)(object)Result.Fail((IEnumerable<IError>)errors);
        }

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = responseType.GetGenericArguments()[0];
            var failMethod = typeof(Result)
                .GetMethod("Fail", 1, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new[] { typeof(IEnumerable<IError>) }, null)!
                .MakeGenericMethod(valueType);

            return (TResponse)failMethod.Invoke(null, new object[] { errors })!;
        }

        throw new InvalidOperationException($"Validation failed but response type {responseType.Name} is not supported. Use Result or Result<T>.");
    }
}
