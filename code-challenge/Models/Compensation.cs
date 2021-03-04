using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        public String compensationId { get; set; }
        public Employee employee { get; set; }
        public float salary { get; set; }
        public String effectiveDate { get; set; }
    }
}
