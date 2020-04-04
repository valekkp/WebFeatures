﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebFeatures.Requests.Internal
{
    internal class PipelineImpl<TRequest, TResponse> : Pipeline<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider services, CancellationToken cancellationToken)
        {
            var handler = services.GetService<IRequestHandler<TRequest, TResponse>>();
            Func<Task<TResponse>> pipeline = () => handler.HandleAsync((TRequest)request, cancellationToken);

            var middlewares = services.GetServices<IRequestMiddleware<TRequest, TResponse>>().Reverse();

            foreach (var middleware in middlewares)
            {
                Func<Task<TResponse>> next = pipeline; // for closure
                pipeline = () => middleware.HandleAsync((TRequest)request, next, cancellationToken);
            }

            return pipeline();
        }
    }
}
