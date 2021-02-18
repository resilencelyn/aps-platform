using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Services
{
    public class ManufactureProcessRepository : RepositoryBase<ApsManufactureProcess, string>
    {
        public ManufactureProcessRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsManufactureProcess> GetAll()
        {
            return base.GetAll().Include(x => x.ApsResources)
                .AsSplitQuery();
        }
    }
}