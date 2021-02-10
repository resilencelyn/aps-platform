using System;

namespace Aps.Shared.Model
{
    public class JobDto
    {
        public int JobId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan Duration { get; set; }
    }
}