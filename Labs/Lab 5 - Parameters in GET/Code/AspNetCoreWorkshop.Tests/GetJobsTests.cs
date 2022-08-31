using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class GetJobsTests : TestBase
    {
        private const string JobsUrl = "/api/jobs";

        [Test]
        public async Task Get_jobs_response_returns_all_data()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();
                var resp = await client.GetAsync(JobsUrl);
                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var json = await resp.Content.ReadFromJsonAsync<IEnumerable<GetJobsResponse>>();
                Assert.That(json.Any());
            }
        }
    }
}