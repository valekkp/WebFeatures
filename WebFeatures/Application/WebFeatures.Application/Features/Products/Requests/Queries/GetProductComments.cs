﻿using System;
using System.Collections.Generic;
using WebFeatures.Application.Features.Products.Dto;
using WebFeatures.Application.Infrastructure.Requests;

namespace WebFeatures.Application.Features.Products.Requests.Queries
{
    /// <summary>
    /// Получить комментарии к товару
    /// </summary>
    public class GetProductComments : IQuery<IEnumerable<ProductCommentInfoDto>>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
    }
}
