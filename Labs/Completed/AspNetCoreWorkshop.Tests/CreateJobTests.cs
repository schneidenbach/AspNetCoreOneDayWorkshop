using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
                        "{}",
                        Encoding.UTF8,
                        "application/json"));

                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                var content = await resp.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            }
        }

        [Test]
        public async Task Create_job_request_with_no_name_returns_bad_request()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();

                var resp = await client.PostAsJsonAsync(
                    "api/jobs", new
                    {
                        Number = "test",
                        StartDate = DateTime.Parse("1/1/2018")
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
                        name = "job test",
                        number = "job test",
                        startDate = DateTime.Parse("1/1/2020")
                    });

                Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var contentJson = (await resp.Content.ReadFromJsonAsync<JsonElement>())
                    .EnumerateObject()
                    .Single(s => s.Name == "id");
                
                Assert.AreNotEqual(contentJson.Value.GetInt32(), 0);
            }
        }
    }
}