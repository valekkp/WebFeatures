﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using WebFeatures.Requests;

namespace WebFeatures.WebApi.Controllers.Base
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IRequestMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IRequestMediator>();
        private IRequestMediator _mediator;

        protected IActionResult Created(object value)
            => StatusCode(StatusCodes.Status201Created, value);
    }
}
