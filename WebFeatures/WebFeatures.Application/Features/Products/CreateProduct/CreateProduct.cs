﻿using FluentValidation;
using System;
using System.IO;
using WebFeatures.Application.Infrastructure.Requests;
using WebFeatures.Application.Interfaces;

namespace WebFeatures.Application.Features.Products.CreateProduct
{
    public class CreateProduct : ICommand<Guid>
    {
        public ProductEditDto Product { get; set; }

        public Stream Picture { get; set; }

        public class Validator : AbstractValidator<CreateProduct>
        {
            public Validator(IWebFeaturesDbContext db)
            {
                RuleFor(x => x.Product.Name).NotEmpty();
                RuleFor(x => x.Product.Description).NotEmpty();

                RuleFor(x => x.Product.ManufacturerId)
                    .MustAsync(async (x, token) => await db.Manufacturers.FindAsync(x) != null);

                RuleFor(x => x.Product.CategoryId)
                    .MustAsync(async (x, token) => !x.HasValue || await db.Categories.FindAsync(x) != null);

                RuleFor(x => x.Product.BrandId)
                    .MustAsync(async (x, token) => await db.Brands.FindAsync(x) != null);
            }
        }
    }
}
