﻿using FluentValidation;
using WebFeatures.Requests;

namespace WebFeatures.Application.Features.Auth.Login
{
    public class Login : ICommand<UserInfoDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class Validator : AbstractValidator<Login>
        {
            public Validator()
            {
            }
        }
    }
}