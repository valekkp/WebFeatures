﻿using WebFeatures.Application.Infrastructure.Requests;
using WebFeatures.Domian.Enums;

namespace WebFeatures.Application.Features.Permissions.Requests
{
    /// <summary>
    /// Проверить наличие разрешения у пользователя
    /// </summary>
    public class UserHasPermission : IQuery<bool>
    {
        public Permission Permission { get; set; }
    }
}