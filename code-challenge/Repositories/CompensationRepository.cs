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
            compensation.compensationId = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            _compensationContext.SaveChanges();
            return compensation;
        }

        public Compensation GetById(string id)
        {

            // Since employee and employee.DirectReports aren't primitive types, we have to explicitly include them in our query.
            Compensation compensation = _compensationContext.Compensations.Include(e => e.employee).ThenInclude(e1 => e1.DirectReports).SingleOrDefaultAsync(c => c.employee.EmployeeId == id).Result;

            return compensation;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
