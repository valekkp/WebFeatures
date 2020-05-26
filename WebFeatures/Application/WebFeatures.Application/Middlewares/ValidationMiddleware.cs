﻿using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFeatures.Application.Exceptions;
using WebFeatures.Requests;

namespace WebFeatures.Application.Middlewares
{
    /// <summary>
    /// Request data validation
    /// </summary>
    internal class ValidationMiddleware<TRequest, TResponse> : IRequestMiddleware<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationMiddleware(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> HandleAsync(TRequest request, RequestDelegate<Task<TResponse>> next, CancellationToken cancellationToken)
        {
            ValidationFailure[] errors =
                (await Task.WhenAll(_validators.Select(x => x.ValidateAsync(request))))
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToArray();

            if (errors.Length != 0)
            {
                throw new RequestValidationException(errors);
            }

            return await next();
        }
    }
}
