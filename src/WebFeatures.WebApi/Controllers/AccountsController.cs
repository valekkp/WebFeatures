﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebFeatures.Application.Features.Accounts.Login;
using WebFeatures.Application.Features.Accounts.Register;
using WebFeatures.Application.Infrastructure.Results;
using WebFeatures.Common;
using WebFeatures.WebApi.Controllers.Base;

namespace WebFeatures.WebApi.Controllers
{
	/// <summary>
	/// Аутентификация
	/// </summary>
	public class AccountsController : BaseController
	{
		/// <summary>
		/// Зарегистрировать нового пользователя
		/// </summary>
		/// <response code="200">Успех</response>
		/// <response code="400" cref="ValidationError">Неверные пользовательские данные</response>
		[HttpPost("[action]")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register([FromBody][Required] RegisterCommand request)
		{
			Guid userId = await Mediator.SendAsync(request);

			await SignInUser(userId);

			return Ok();
		}

		/// <summary>
		/// Войти на сайт
		/// </summary>
		/// <response code="200">Успех</response>
		/// <response code="400" cref="ValidationError">Неверные пользовательские данные</response>
		[HttpPost("[action]")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Login([FromBody][Required] LoginCommand request)
		{
			Guid userId = await Mediator.SendAsync(request);

			await SignInUser(userId);

			return Ok();
		}

		private async Task SignInUser(Guid userId)
		{
			Claim[] claims =
			{
				new Claim(ClaimTypes.NameIdentifier, userId.ToString())
			};

			var identity = new ClaimsIdentity(
				claims,
				CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				IsPersistent = true,
				ExpiresUtc = SystemTime.Now.AddMinutes(20)
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(identity),
				authProperties);
		}

		/// <summary>
		/// Выйти из сайта
		/// </summary>
		/// <response code="200">Успех</response>
		[HttpPost("[action]")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return Ok();
		}
	}
}
