using System;

namespace Aps.Shared.Entity
{
    public class ApsJobApsResource
    {
        public ApsResource ApsResource { get; set; }
        public string ApsResourceId { get; set; }

        public ApsJob ApsJob { get; set; }
        public Guid ApsJobId { get; set; }
    }
}