using System;
using System.Collections.Generic;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;

namespace Aps.Shared.Model
{
    public class ScheduleRecordDto
    {
        public Guid Id { get; set; }


        public ICollection<OrderDto> Orders { get; set; }
        
        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleFinishTime { get; set; }
        
        public ICollection<JobDto> Jobs { get; set; } = new List<JobDto>();

        public ICollection<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
        
        public RecordState RecordState { get; set; }
    }

    
}