using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class CreateJobPhaseForJobTests : TestBase
    {
        [Test]
        public async Task Create_job_phase_for_job_succeeds_with_valid_request()
        {
            using (var server = CreateTestServer())
            {
                Assert.Fail();
            }
        }
        
        [Test]
        public async Task Create_job_phase_for_job_returns_400_when_no_number_and_description_specified()
        {
            using (var server = CreateTestServer())
            {
                Assert.Fail();
                //TODO: Assert that Number and Description are both in the errors returned.
            }
        }
        
        [Test]
        public async Task Create_job_phase_for_job_returns_400_when_number_exists_on_phase_for_job()
        {
            using (var server = CreateTestServer())
            {
                Assert.Fail();
                //TODO: Assert that Number are both in the errors returned.
            }
        }
    }
}