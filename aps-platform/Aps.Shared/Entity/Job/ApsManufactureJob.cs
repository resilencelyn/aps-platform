using System;

namespace Aps.Shared.Entity
{
    public class ApsManufactureJob : ApsJob
    {
        public ApsSemiProduct ApsSemiProduct { get; set; }
        public SemiProductInstance SemiProductInstance { get; set; }
        public ApsManufactureProcess ApsManufactureProcess { get; set; }

        public ApsJob PreJob { get; set; }

        public Guid PreJobId { get; set; }
        public Guid BatchId { get; set; }

        
        public ScheduleRecord ScheduleRecord { get; set; }

    }
}