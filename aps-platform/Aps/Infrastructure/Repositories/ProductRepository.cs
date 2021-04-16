using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Services
{
    public class ProductRepository : RepositoryBase<ApsProduct, string>
    {
        public ProductRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsProduct> GetAll()
        {
            return base.GetAll()
                .Include(x => x.AssembleBySemiProducts)
                .Include(x => x.ApsAssemblyProcess)
                .AsSplitQuery();
        }

        public override ApsProduct Insert(ApsProduct entity)
        {
            if (entity.AssembleBySemiProducts.Any())
            {
                foreach (var entityAssembleBySemiProduct in entity.AssembleBySemiProducts)
                {
                    entityAssembleBySemiProduct.ApsProductId = entity.Id;
                }
            }

            var productAdd = Table.Add(entity).Entity;
            Save();

            return productAdd;
        }

        // public override async Task<ApsProduct> InsertAsync(ApsProduct entity)
        // {
        //     if (entity.AssembleBySemiProducts.Any())
        //     {
        //         foreach (var entityAssembleBySemiProduct in entity.AssembleBySemiProducts)
        //         {
        //             entityAssembleBySemiProduct.ApsProductId = entity.Id;
        //         }
        //     }
        //
        //     var productAddEntry = await Table.AddAsync(entity);
        //     await SaveAsync();
        //
        //     return productAddEntry.Entity;
        // }
        //
        // public override void Delete(ApsProduct entity)
        // {
        //     _apsContext.RemoveRange(entity.AssembleBySemiProducts);
        //     base.Delete(entity);
        // }
    }
}