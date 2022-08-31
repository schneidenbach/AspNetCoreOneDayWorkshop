# Add security

The goal of this lab is to practice using the security features inside of ASP.NET Core.

## Concepts

- Implement a not-production-level security key endpoint.
- Implement authorization on our controllers to secure endpoints.

## Tasks

0. Run the `SecurityTests` unit test. You should see it fail - that's ok for now. Let's make it pass!
1. Change the `Configure` method in Startup.cs to be the following:

```csharp
public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(builder =>
    {
        builder.MapControllers();
    });
}
```

2. Install the `Microsoft.AspNetCore.Authentication.JwtBearer` NuGet package on the AspNetCoreWorkshop.Api (in the provided lab, this has already been done for you).
3. Add the following to the JSON object inside of appsettings.json in both the test and API projects:
```json
"Tokens": {
  "Issuer": "https://localhost:5001",
  "Key": "some-long-secret-key"
}
```

It should look something like this:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Tokens": {
    "Issuer": "https://localhost:5001",
    "Key": "some-long-secret-key"
  }
}

```

4. Add the following lines to the Startup.cs:
```csharp
public IConfiguration Configuration { get; }

public Startup(IConfiguration configuration)
{
    Configuration = configuration;
}
```

This exposes the application configuration to your startup methods.

5. Add the following line in your `ConfigureServices` method in Startup.cs.

```csharp
services.AddJwtBearerAuthentication(Configuration);
```

This adds the required JWT services to your application. (If your tooling doesn't do it for you, add `using AspNetCoreWorkshop.Api.Security;` to your usings declaration at the top of the file.)

6. Run the tests for SecurityTests. You should now see them pass! This means your endpoint for generating tokens will work now.
7. Add the following attribute to your `JobsController`:

```csharp
[Authorize]
```
8. Add the following attribute to the endpoints related to JobPhases to your `JobsController`:

```csharp
[Authorize(Roles = "project manager")]
```

Your `JobsController` should now look something like this:

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JobsController : WorkshopController
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetJob([FromRoute] GetJobRequest request)
    {
        return HandleRequestAsync(request);
    }

    [HttpGet]
    public Task<IActionResult> GetJobs([FromQuery] GetJobsRequest request)
    {
        return HandleRequestAsync(request);
    }

    [HttpPost]
    public Task<IActionResult> CreateJob(CreateJobRequest request)
    {
        return HandleRequestAsync(request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, JsonElement body)
    {
        var updateJobRequest = new UpdateJobRequest(id, body);

        if (!TryValidateModel(updateJobRequest))
        {
            return BadRequest(ModelState);
        }
        
        return await HandleRequestAsync(updateJobRequest);
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteJob(int id)
    {
        return HandleRequestAsync(new DeleteJobRequest(id));
    }

    [Authorize(Roles = "project manager")]
    [HttpGet("{jobId}/phases")]
    public Task<IActionResult> GetJobPhasesForJob([FromRoute] GetJobsPhasesForJobRequest request)
    {
        return HandleRequestAsync(request);
    }

    [Authorize(Roles = "project manager")]
    [HttpPost("{jobId}/phases")]
    public async Task<IActionResult> GetJobPhasesForJob(int jobId, [FromBody] CreateJobPhaseForJobRequestBody request)
    {
        var body = new CreateJobPhaseForJobRequest(jobId, request?.Number, request?.Description);
        if (!TryValidateModel(body))
        {
            return BadRequestAsValidationProblemDetails();
        }
        return await HandleRequestAsync(body);
    }
}
```

9. Run your entire suite of unit tests. See all those failures?! Those are to be expected! You should see lots of your tests failing due to Unauthorized errors.
10. Go to TestBase.cs and look at the `AddAuthorizationToClientForRoleAsync` method. Note what it's doing - it's taking a given role and generating a token using an endpoint in the `SecurityController`, which was reviewed before we started the lab.
11. In a few of your Job tests, add the following line after creating your `HttpClient` but before you make any other calls:

```csharp
await AddAuthorizationToClientForRoleAsync(client, "bricklayer");
```

For example, if you did this in the `GetJobTests` file to both tests, it would look something like this:

```csharp
public class GetJobTests : TestBase
{
    private const string JobsUrl = "/api/jobs";

    [Test]
    public async Task Get_jobs_response_returns_not_found_when_job_does_not_exist()
    {
        using (var server = CreateTestServer())
        {
            var client = server.CreateClient();
            await AddAuthorizationToClientForRoleAsync(client, "bricklayer");
            
            var resp = await client.GetAsync(JobsUrl + "/100000");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [Test]
    public async Task Get_job_response_returns_correct_data()
    {
        using (var server = CreateTestServer())
        {
            var context = server.Host.Services.GetService<WorkshopDbContext>();
            var job = context.Jobs.First();
            
            var client = server.CreateClient();
            await AddAuthorizationToClientForRoleAsync(client, "bricklayer");
            var requestUri = new Uri($"{JobsUrl}/{job.Id}", UriKind.Relative);

            var resp = await client.GetAsync(requestUri);
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var json = await resp.Content.ReadFromJsonAsync<GetJobResponse>();
            Assert.That(json.Name, Is.EqualTo(job.Name));
            Assert.That(json.Number, Is.EqualTo(job.Number));
        }
    }
}
```
12. Rerun the tests. They should now pass.
13. Do the same in some JobPhase tests. You should still see those tests fail, but this time they fail with a Forbidden error. Change "bricklayer" to "project manager" and rerun the tests. They should now pass.

For example, if you did this in the `GetJobPhasesTests` class, it would look something like this before you changed bricklayer to project manager:

```csharp
public class GetJobPhasesTests : TestBase
{
    [Test]
    public async Task Get_job_phases_response_returns_all_data()
    {
        using (var server = CreateTestServer())
        {
            var context = server.Host.Services.GetService<WorkshopDbContext>();
            var job = context.Jobs.Single(j => j.Number == "George");
            
            var client = server.CreateClient();
            await AddAuthorizationToClientForRoleAsync(client, "bricklayer");
            var resp = await client.GetAsync($"api/jobs/{job.Id}/phases");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var json = await resp.Content.ReadFromJsonAsync<IEnumerable<GetJobPhasesForJobResponse>>();
            Assert.That(json.Count, Is.EqualTo(3));
            Assert.That(json.Any(j => j.Number == "0046"));
        }
    }

    [Test]
    public async Task Get_job_phases_returns_404_for_non_existent_job()
    {
        using (var server = CreateTestServer())
        {
            var client = server.CreateClient();
            await AddAuthorizationToClientForRoleAsync(client, "bricklayer");
            var resp = await client.GetAsync("api/jobs/100000/phases");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
```

14. Start your application and use Postman to experiment with your newly-secured APIs. Generate tokens by doing POST requests to `https://localhost:5001/security/generateToken` with a JSON body that looks something like this:

```json
{
    "role": "project manager"
}
```

Copy the token that's provided in the response.

15. Then, try and do a GET request against the `https://localhost:5001/api/jobs` endpoint without the token. It should fail with a 401 Unauthorized.
16. In your GET request, click the Authorization tab and select "Bearer Token" from the Type dropdown, then paste the token you got from step 14 into your request and click Send. The request should now succeed.

## Acknowledgements

The example here was adapted from https://github.com/miroslavpopovic/aspnetcore-workshop/blob/master/docs/07-securing-api.md.