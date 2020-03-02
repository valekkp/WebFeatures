﻿using System.Linq;
using WebFeatures.Domian.Entities.Abstractions;

namespace WebFeatures.Application.Interfaces.DataAccess
{
    public interface IRepository<TEntity, in TId> 
        where TEntity : BaseEntity<TId> 
        where TId : struct
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(TId id);

        bool Exists(TId id);

        void Add(TEntity entity);

        void Remove(TId id);

        void SaveChanges();
    }
}