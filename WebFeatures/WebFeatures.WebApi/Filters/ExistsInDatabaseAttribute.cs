﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WebFeatures.Application.Interfaces.DataAccess;
using WebFeatures.Domian.Entities.Abstractions;

namespace WebFeatures.WebApi.Filters
{
    public class ExistsInDatabaseAttribute : ActionFilterAttribute
    {
        private readonly Type _repoType;

        public ExistsInDatabaseAttribute(Type entityType)
        {
            if (entityType.BaseType == null ||
                entityType.BaseType == typeof(object) ||
                !entityType.BaseType.IsGenericType ||
                entityType.BaseType.GetGenericTypeDefinition() != typeof(BaseEntity<>))
                throw new ArgumentException(nameof(entityType));

            _repoType = typeof(IRepository<,>).MakeGenericType(
                entityType,
                entityType.BaseType.GetGenericArguments()[0]);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("id", out var idVal))
            {
                dynamic repo = context.HttpContext.RequestServices.GetService(_repoType);
                if (!repo.Exists(idVal))
                {
                    context.Result = new NotFoundObjectResult(idVal);
                }
            }
        }
    }
}