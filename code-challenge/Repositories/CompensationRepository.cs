using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using challenge.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            // Compensation needs a primary key, but since each employee will have just one compensation,
            // we can use the EmployeeId as the primary key.
            compensation.compensationId = compensation.employee.EmployeeId;

            _compensationContext.Compensations.Add(compensation);
            _compensationContext.SaveChanges();
            return compensation;
        }

        public Compensation GetById(string id)
        {

            // We need to remember to include the compensation's employee property.
            List<Compensation> compensations = _compensationContext.Compensations
                                              .Include(c => c.employee)
                                              .ToListAsync().Result;

            Compensation compensation = compensations.SingleOrDefault(c => c.employee.EmployeeId == id);

            return compensation;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
