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

        [Test]
        public async Task Get_jobs_response_returns_data_sorted_correctly()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();

                var requestUri = new Uri($"{JobsUrl}?orderBy=Number", UriKind.Relative);

                var resp = await client.GetAsync(requestUri);
                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var json = await resp.Content.ReadFromJsonAsync<IEnumerable<GetJobsResponse>>();
                Assert.That(json.First().Number == "12345-");
                Assert.That(json.Last().Number == "George");
            }
        }

        [Test]
        public async Task Get_job_response_returns_data_filtered_by_number()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();
                var requestUri = new Uri($"{JobsUrl}?Number=1234", UriKind.Relative);

                var resp = await client.GetAsync(requestUri);
                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var json = await resp.Content.ReadFromJsonAsync<IEnumerable<GetJobsResponse>>();
                Assert.That(json.Count() == 1);
                
                var job = json.Single();
                Assert.That(job.Number == "12345-");
            }
        }
    }
}