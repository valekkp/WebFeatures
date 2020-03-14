﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Reflection;
using WebFeatures.Domian.Common;
using WebFeatures.Domian.Entities;

namespace WebFeatures.DataContext
{
    public class WebFeaturesDbContext : DbContext
    {
        public WebFeaturesDbContext(DbContextOptions<WebFeaturesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SetupUtcConverters(modelBuilder);
            SetupSoftDelete(modelBuilder);
        }

        private void SetupUtcConverters(ModelBuilder modelBuilder)
        {
            var utcConverter = new ValueConverter<DateTime, DateTime>(
                to => to,
                from => DateTime.SpecifyKind(from, DateTimeKind.Utc));

            var nullableUtcConverter = new ValueConverter<DateTime?, DateTime?>(
                to => to,
                from => from != default ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : default);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.Name.EndsWith("Utc"))
                    {
                        if (property.ClrType == typeof(DateTime))
                        {
                            property.SetValueConverter(utcConverter);
                        }

                        if (property.ClrType == typeof(DateTime?))
                        {
                            property.SetValueConverter(nullableUtcConverter);
                        }
                    }
                }
            }
        }

        private void SetupSoftDelete(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    typeof(WebFeaturesDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });

                    //var lambda = BuildSoftDeleteLambda(entityType.ClrType);
                    //entityType.SetQueryFilter(lambda);
                }
            }
        }

        private void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
        {
            modelBuilder.Entity<T>().HasQueryFilter(x => !x.IsDeleted);
        }

        //private LambdaExpression BuildSoftDeleteLambda(Type type)
        //{
        //    var parameter = Expression.Parameter(type, "x");

        //    var body = Expression.Not(
        //        expression: Expression.Property(
        //            expression: parameter,
        //            propertyName: nameof(ISoftDelete.IsDeleted)));

        //    return Expression.Lambda(body, new[] { parameter });
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
    }
}
