using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Services
{
    public class ResourceRepository : RepositoryBase<ApsResource, string>
    {
        public ResourceRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsResource> GetAll()
        {
            return base.GetAll().Include(r => r.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass);
        }
    }
}