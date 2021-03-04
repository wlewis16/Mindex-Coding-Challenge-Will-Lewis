using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure CreateReportingStructure(string id)
        {
            if (String.IsNullOrEmpty(id))
                return null;

            Employee employee = _employeeRepository.GetById(id).Result;

            if (employee != null)
                return CreateReportingStructureHelper(employee);
            else
                return null;
        }

        /// <summary>
        /// The structure outlined in the Coding Challenge describes an N-ary tree, so we'll do preorder tree traversal.
        /// </summary>
        /// <param name="employee">The root employee</param>
        /// <returns>A ReportingStructure with the root employee at the top.</returns>
        private ReportingStructure CreateReportingStructureHelper(Employee employee)
        {
            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.employee = employee;

            if(employee.DirectReports == null)
            {
                reportingStructure.numberOfReports = 0;
                return reportingStructure;
            }

            Stack<Employee> employees = new Stack<Employee>();
            int numDirectReports = -1;

            employees.Push(employee);

            while (employees.Count != 0)
            {
                Employee currEmployee = employees.Pop();

                if(currEmployee != null)
                {
                    numDirectReports += 1;

                    if (currEmployee.DirectReports != null)
                    {
                        for (int idx = 0; idx < currEmployee.DirectReports.Count; idx++)
                        {
                            employees.Push(currEmployee.DirectReports[idx]);
                        }
                    }
                }
            }

            reportingStructure.numberOfReports = numDirectReports;

            return reportingStructure;
        }
    }
}
