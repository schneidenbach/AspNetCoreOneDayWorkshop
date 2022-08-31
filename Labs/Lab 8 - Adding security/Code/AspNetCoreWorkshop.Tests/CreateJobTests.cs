using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs.CreateJob;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class CreateJobTests : TestBase
    {
        [Test]
        public async Task Create_job_blank_request_returns_bad_request()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();

                var resp = await client.PostAsync(
                        "api/jobs",
                    new StringContent(
                        string.Empty,
                        Encoding.UTF8,
                        "application/json"));

                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async Task Create_job_request_with_no_name_returns_bad_request()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();

                var resp = await client.PostAsJsonAsync(
                    "api/jobs",
                    new
                    {
                        Number = "test",
                        StartDate = "2020-01-01"
                    });

                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

                var contentJson = await resp.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                Assert.That(contentJson, Is.Not.Null);
                Assert.That(contentJson.Errors.ContainsKey("Name"), Is.True);
            }
        }

        [Test]
        public async Task Create_jobs_correct_request_returns_ok_with_id()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();

                var resp = await client.PostAsJsonAsync(
                    "api/jobs",
                    new
                    {
                        Name = "job test",
                        Number = "job test",
                        StartDate = "2020-01-01"
                    });

                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var contentJson = await resp.Content.ReadFromJsonAsync<CreateJobResponse>();
                Assert.AreNotEqual(contentJson.Id, 0);
            }
        }
    }
}