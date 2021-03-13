using System;
using System.Collections.Generic;
using Aps.Shared.Extensions;

namespace Aps.Shared.Model
{
    public class JobDto
    {
        public string Id { get; set; }
        public string ApsOrderId { get; set; }
        public string ApsProductId { get; set; }
        public string ProductInstanceId { get; set; }


        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Duration { get; set; }

        public Workspace Workspace { get; set; }
        public ICollection<ResourceDto> ApsResource { get; set; } = new List<ResourceDto>();
    }
}