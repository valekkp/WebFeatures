﻿using AutoMapper;
using FluentValidation;
using System;
using WebFeatures.Application.Infrastructure.Mappings;
using WebFeatures.Application.Infrastructure.Requests;
using WebFeatures.Application.Infrastructure.Results;
using WebFeatures.Application.Interfaces.DataAccess;
using WebFeatures.Domian.Entities;

namespace WebFeatures.Application.Features.Products.Requests.Commands
{
    /// <summary>
    /// Редактировать товар
    /// </summary>
    public class UpdateProduct : ICommand<Empty>, IHasMappings
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор производителя
        /// </summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        public Guid BrandId { get; set; }

        public void ApplyMappings(Profile profile)
        {
            profile.CreateMap<UpdateProduct, Product>(MemberList.Source);
        }

        public class Validator : AbstractValidator<UpdateProduct>
        {
            public Validator(IWriteDbContext db)
            {
                RuleFor(p => p.Id)
                    .MustAsync(async (x, t) => await db.Products.ExistsAsync(x));

                RuleFor(p => p.Name)
                    .NotEmpty();

                RuleFor(p => p.Description)
                    .NotEmpty();

                RuleFor(p => p.ManufacturerId)
                    .MustAsync(async (x, t) => await db.Manufacturers.ExistsAsync(x));

                RuleFor(p => p.CategoryId)
                    .MustAsync(async (x, t) => await db.Categories.ExistsAsync(x.Value))
                    .When(p => p.CategoryId.HasValue);

                RuleFor(p => p.BrandId)
                    .MustAsync(async (x, t) => await db.Brands.ExistsAsync(x));
            }
        }
    }
}