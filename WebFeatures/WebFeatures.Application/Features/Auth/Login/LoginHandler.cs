﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebFeatures.Application.Exceptions;
using WebFeatures.Application.Features.Auth.RegisterUser;
using WebFeatures.Application.Interfaces;
using WebFeatures.Application.Interfaces.DataContext;
using WebFeatures.Domian.Entities;
using WebFeatures.Requests;

namespace WebFeatures.Application.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<Login, UserInfoDto>
    {
        private readonly IWriteContext _db;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly IMapper _mapper;

        public LoginHandler(
            IWriteContext db,
            IPasswordEncoder passwordEncoder,
            ILogger<RegisterUserHandler> logger,
            IMapper mapper)
        {
            _db = db;
            _passwordEncoder = passwordEncoder;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> HandleAsync(Login request, CancellationToken cancellationToken)
        {
            User user = await _db.Users
                .AsNoTracking()
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
                throw new ApplicationValidationException("Wrong login or password");

            string password = _passwordEncoder.DecodePassword(user.PasswordHash);
            if (password != request.Password)
                throw new ApplicationValidationException("Wrong login or password");

            _logger.LogInformation($"{user.Email} logged in");

            return _mapper.Map<UserInfoDto>(user);
        }
    }
}
