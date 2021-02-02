using System;
using System.ComponentModel.DataAnnotations;

namespace Aps.Entity
{
    public class ApsProcessResource
    {
        public string ApsProcessId { get; set; }
        public ApsProcess ApsProcess { get; set; }
        public string ResourceAttribute { get; set; }

        public int Amount { get; set; }
    }
}