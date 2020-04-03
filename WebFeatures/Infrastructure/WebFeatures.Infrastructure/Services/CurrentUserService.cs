﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebFeatures.Application.Interfaces;

namespace WebFeatures.Infrastructure.Services
{
    internal class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor contexAccessor)
        {
            HttpContext context = contexAccessor.HttpContext;

            if (context?.User?.Identity != null && context.User.Identity.IsAuthenticated)
            {
                UserId = Guid.Parse(
                    context.User.FindFirstValue(ClaimTypes.NameIdentifier));

                Roles = context.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value);

                IsAuthenticated = true;
            }
        }

        public bool IsAuthenticated { get; }
        public Guid UserId { get; }
        public IEnumerable<string> Roles { get; }
    }
}