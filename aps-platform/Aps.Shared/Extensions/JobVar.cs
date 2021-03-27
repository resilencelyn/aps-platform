using System;
using System.ComponentModel.DataAnnotations.Schema;
using Google.OrTools.Sat;

namespace Aps.Shared.Entity
{
    [NotMapped]
    public class JobVar
    {
        public IntVar StartVar { get; set; }
        public IntVar EndVar { get; set; }
        public IntervalVar Interval { get; set; }

        public void Deconstruct(out IntVar startVar, out IntVar endVar, out IntervalVar interval)
        {
            startVar = StartVar;
            endVar = EndVar;
            interval = Interval;
        }

        public JobVar(IntVar startVar, IntVar endVar, IntervalVar interval)
        {
            StartVar = startVar ?? throw new ArgumentNullException(nameof(startVar));
            EndVar = endVar ?? throw new ArgumentNullException(nameof(endVar));
            Interval = interval ?? throw new ArgumentNullException(nameof(interval));
        }
    }
}