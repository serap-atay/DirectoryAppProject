﻿using DirectoryApp.Models.Entities;

namespace DirectoryApp.Repository.Abstracts
{
    public interface IRepository<T, in TId> where T : BaseEntity
    {
        T GetById(TId id);
        IQueryable<T> Get(Func<T, bool> predicate = null);
        void Add(T entity, bool isSaveLater = false);
        void Remove(T entity, bool isSaveLater = false);
        void Update(T entity, bool isSaveLater = false);
        int Save();
    }
}
