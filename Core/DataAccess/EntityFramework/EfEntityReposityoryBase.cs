using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityReposityoryBase <TEntity, TContext> :IEntityRepository<TEntity>
        where TEntity: class,IEntity,new()
        where TContext: DbContext,new()
        //dbcontext tipinde olacak ve newlenebilir olacak.
    {
        public void Add(TEntity entity)
        {
            //usin bitince garbage collector iş bitince bunu atacak.
            //IDisposable pattern implementation of C#
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);  //entity i db ye bağla.
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);  //entity i db ye bağla.
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                        ? context.Set<TEntity>().ToList()
                        : context.Set<TEntity>().Where(filter).ToList();
            }
        }
        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);  //entity i db ye bağla.
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
