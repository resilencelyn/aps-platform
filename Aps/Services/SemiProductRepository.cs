using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Services
{
    public class SemiProductRepository : RepositoryBase<ApsSemiProduct, string>
    {
        public SemiProductRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsSemiProduct> GetAll()
        {
            return base.GetAll().Include(x => x.ApsManufactureProcesses)
                .AsSplitQuery();
        }
    }
}