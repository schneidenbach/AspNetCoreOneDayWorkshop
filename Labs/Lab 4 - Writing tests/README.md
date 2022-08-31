# Test the GET `/jobs` endpoint

The goal of this is to learn how to create easy-to-write tests for your API project.

## Concepts

- Learn to leverage TestHost to create your server and execute requests.
- Learn how to hydrate your data stores in order to properly setup your test conditions.
- Run your ASP.NET tests.

## Tasks

### Setup the base test

1. Install the `Microsoft.AspNetCore.TestHost` NuGet package (in the sample provided, this has already been done).
2. Add a TestBase class to the AspNetCoreWorkshop.Tests project.
3. Add the following code and import all required NuGet packages:

```csharp
protected TestServer CreateTestServer()
{
    var hostBuilder = new WebHostBuilder()
        .UseStartup<Startup>();

    return new TestServer(hostBuilder);
}
```

4. Add a GetJobsTest class to the test project that inherits from the TestBase.
5. Add the following code to the GetJobsTest class:

```csharp
[Test]
public async Task Get_jobs_response_returns_all_data()
{
    using (var server = CreateTestServer())
    {
        var client = server.CreateClient();
        var resp = await client.GetAsync("/api/jobs");
        Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await resp.Content.ReadFromJsonAsync<IEnumerable<GetJobsResponse>>();
        Assert.That(!json.Any());
    }
}
```

6. Run the test - either using your favorite tool or `dotnet test` on the command line.
7. If all goes correctly, your test will pass - but it doesn't do much yet because it's not testing for data.

### Hydrate the data store

1. In order to hook into the data store, we'll hook up an in-memory WorkshopDbContext with some seed data to ensure that our API works. Add the following code after the call to `new WebHostBuilder()`:

```csharp
.ConfigureServices(services =>
{
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    services.AddDbContext<WorkshopDbContext>(options =>
    {
        options.UseInMemoryDatabase("test");
        options.UseInternalServiceProvider(serviceProvider);
    });

    var sp = services.BuildServiceProvider();

    using (var scope = sp.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<WorkshopDbContext>();

        db.Database.EnsureCreated();

        db.Jobs.Add(new Job
        {
            Number = "12345-",
            Name = "Building a Wal-Mart",
            Description = null,
            StartDate = new DateTime(2019, 5, 15)
        });

        db.Jobs.Add(new Job
        {
            Number = "George",
            Name = "George's Day Spa",
            Description = null,
            StartDate = new DateTime(2019, 7, 15)
        });

        db.SaveChanges();
    }
})
```

### Refactor your test

1. Rerun your test - it should now be failing due to this line:

```csharp
Assert.That(!json.Any());
```

2. Change it to assert any/all of the following:

- The count of records is equal to the number of the items in the 
- All job numbers of all records in the context are contained in the result set.
- The result set contains results at all. :)

I wouldn't worry about order of the results - we'll check that out in a future lab.

HINT: you can access a copy of the WorkshopDbContext inside of your test by calling `server.Host.Services.GetService<WorkshopDbContext>()`.