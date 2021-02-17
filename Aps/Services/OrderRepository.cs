using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Services
{
    public class OrderRepository : RepositoryBase<ApsOrder, string>
    {
        public OrderRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ApsOrder> GetAll()
        {
            return base.GetAll().Include(o => o.Product);
        }
    }
}