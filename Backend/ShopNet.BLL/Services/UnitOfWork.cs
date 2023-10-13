using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Abstract;
using ShopNet.DAL.Data;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Services
{
    public class UnitOfWork : BaseService, IUnitOfWork
    {
        private readonly StoreContext context;
        private Hashtable repositories;

        public UnitOfWork(StoreContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            repositories ??= new Hashtable();

            var type = typeof(GenericRepository<TEntity>);
            var contains = !repositories.ContainsKey(type);
            if (!repositories.ContainsKey(type))
            {
                var repoInstance = Activator.CreateInstance(type, context);
                repositories.Add(type, repoInstance);
            }
            var repo = (IGenericRepository<TEntity>)repositories[type];
            return repo;
        }
    }
}