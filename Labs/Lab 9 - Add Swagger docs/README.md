# Add security

The goal of this lab is to practice adding Swagger docs to our API endpoint.

## Concepts

- Install and use Swagger to automatically document our APIs.

## Tasks

0. Start by running the tests and notice that the `EndpointDocumentationTests` test is failing. Let's fix it!
1. Install the `NSwag.AspNetCore` Nuget package.
2. Add the following lines to the `ConfigureServices` method in Startup.cs:

```csharp
services.AddSwaggerDocument(options =>
{
    options.DocumentName = "Project Management API";
    options.OperationProcessors.Add(new OperationSecurityScopeProcessor("jwt-token"));
    options.DocumentProcessors.Add(new SecurityDefinitionAppender(
        "jwt-token", new[] {""}, new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            Description =
                "Add a bearer token to your request. Use POST https://localhost:5001/security/generateToken to generate a token.",
            In = OpenApiSecurityApiKeyLocation.Header
        })
    );
});
services.AddOpenApiDocument();
```

3. Add the following lines in Startup.cs in the `Configure` method after the call to `UseAuthorization`:

```csharp
app.UseOpenApi();
app.UseSwaggerUi3();
```

4. Start the application and navigate to https://localhost:5001/swagger.
5. Use Postman OR the Swagger interface to generate a token using the POST https://locahost:5001/security/generateToken endpoint (don't forget to specify a role in the request body like below:)

```json
{
    "role": "anything"
}
```

You can use the Swagger interface by clicking on "Security", then the `POST /security/generateToken` header. Click the "Try it out" button and then click "Execute". 

6. Copy the generated token (if you generate it in Swagger, be sure NOT to copy the quotes).
7. At the top of the Swagger UI, click the Authorize button. In the "Value" textbox, type "Bearer " (be sure to include the space) then paste in your code you copied in step 6.
8. Start interacting with the API using the Swagger interface.
9. Once you're done messing around with Swagger, stop the application and go to the JobsController.
10. Add the following attributes to the `GetJob` endpoint:

```csharp
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobResponse))]
[ProducesResponseType(StatusCodes.Status404NotFound)]
```
11. Restart the app and navigate to Swagger and see how the document changes.
12. Stop the application and add an XML comment to the top of the method (you can do this by typing three backslashes `///`).
13. Type something into the summary part of the XML. Your endpoint should look something like this:

```csharp
/// <summary>
/// Gets a job with a specific ID.
/// </summary>
[HttpGet("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobResponse))]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public Task<IActionResult> GetJob([FromRoute] GetJobRequest request)
{
    return HandleRequestAsync(request);
}
```

14. Open your csproj file and add the following line under `AssemblyName`:
```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
```

15. Restart the app and observe how the Swagger documentation has improved by adding your XML comment.
16. Add at least one `[ProducesResponseType]` attribute to each endpoint.
17. Rerun the `EndpointDocumentationTests` and watch it pass!