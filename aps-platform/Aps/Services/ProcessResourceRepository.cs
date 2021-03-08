using System.Linq;
using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;

namespace Aps.Services
{
    public class ProcessResourceRepository : RepositoryBase<ApsProcessResource, string>
    {
        public ProcessResourceRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsProcessResource> GetAll()
        {
            return base.GetAll().Include(x => x.ResourceClass);
        }
    }
}