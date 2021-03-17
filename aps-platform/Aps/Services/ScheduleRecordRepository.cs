using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Aps.Infrastructure;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;

namespace Aps.Services
{
    public class ScheduleRecordRepository : RepositoryBase<ScheduleRecord, Guid>
    {
        public ScheduleRecordRepository(ApsContext apsContext) : base(apsContext)
        {
        }

        public override IQueryable<ScheduleRecord> GetAll()
        {
            return base.GetAll()
                .AsNoTracking()
                .Include(x => x.Jobs)
                .ThenInclude(x => x.ApsResource)
                
                .Include(x => x.Orders)
                .AsSplitQuery();
        }

        public override ScheduleRecord FirstOrDefault(Expression<Func<ScheduleRecord, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public override Task<ScheduleRecord> FirstOrDefaultAsync(Expression<Func<ScheduleRecord, bool>> predicate)
        {
            return GetAll().FirstOrDefaultAsync(predicate);
        }
    }
}