using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api;
using AspNetCoreWorkshop.Api.JobPhases.GetJobPhasesForJob;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class GetJobPhasesTests : TestBase
    {
        [Test]
        public async Task Get_job_phases_response_returns_all_data()
        {
            using (var server = CreateTestServer())
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task Get_job_phases_returns_404_for_non_existent_job()
        {
            Assert.Fail();
        }
    }
}