using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using challenge.Models;

namespace challenge.Data
{
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options): base(options)
        { }

        public DbSet<Compensation> Compensations { get; set; }
    }
}
