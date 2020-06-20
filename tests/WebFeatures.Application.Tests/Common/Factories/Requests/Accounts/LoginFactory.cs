﻿using Bogus;
using WebFeatures.Application.Features.Accounts.Requests.Commands;

namespace WebFeatures.Application.Tests.Common.Factories.Requests.Accounts
{
    internal static class LoginFactory
    {
        public static Login Get()
        {
            var login = new Faker<Login>()
                .StrictMode(true)
                .RuleFor(x => x.Email, x => x.Internet.Email())
                .RuleFor(x => x.Password, x => x.Internet.Password());

            return login;
        }
    }
}