using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;
using System;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {

            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                salary = 10000,
                effectiveDate = "01-01-2000"
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            var postRequestTask = _httpClient.PostAsync($"api/compensation/{employeeId}",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));

            var response = postRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.compensationId);
            Assert.IsNotNull(newCompensation.employee);
            Assert.AreEqual(newCompensation.employee.EmployeeId, employeeId);
            Assert.AreEqual(newCompensation.salary, compensation.salary);
            Assert.AreEqual(newCompensation.effectiveDate, compensation.effectiveDate);
        }

        [TestMethod]
        public void GetCompensation_Returns_Ok()
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedSalary = 10000;
            var expectedEffectiveDate = "01-01-2000";

            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, compensation.employee.EmployeeId);
            Assert.AreEqual(compensation.salary, expectedSalary);
            Assert.AreEqual(compensation.effectiveDate, expectedEffectiveDate);
        }

        [TestMethod]
        public void GetCompensation_Returns_NotFound()
        {
            var employeeId = Guid.NewGuid().ToString();

            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
