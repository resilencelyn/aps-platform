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
                .Include(x => x.ApsAssemblyProcess);
        }
    }
}