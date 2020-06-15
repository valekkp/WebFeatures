﻿using System;
using WebFeatures.Application.Infrastructure.Requests;

namespace WebFeatures.Application.Features.Accounts.Requests.Commands
{
    /// <summary>
    /// Войти в систему
    /// </summary>
    public class Login : ICommand<Guid>
    {
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}