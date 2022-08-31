# Add parameters for filtering your GET /jobs endpoint

The goal of this is to learn about how using this pattern of development makes refactoring and further testing of your API changes easier, faster, and safer. We'll do this by adding two parameters to our GET /jobs endpoint: orderBy (to properly sort our results) and number (as a filter on the Number property).

## Concepts

- Learn how to add parameters to your GET requests.
- Learn how to refactor your handler to use your parameters.
- Learn how to incrementally refactor your tests.

## Tasks

### Extend your request model

1. Add the following string properties to your `GetJobsRequest` class: OrderBy, Number

### Add request parameters

1. Refactor the `GetJobs` method to look like this:

```csharp
public Task<IActionResult> GetJobs([FromQuery] GetJobsRequest request)
{
    return HandleRequestAsync(request);
}
```

The `[FromQuery]` attribute will tell ASP.NET to pull the parameters from the query string into the property values of the class.

### Refactor your validator

You need to validate that the OrderBy property contains a valid property name. Ensure that the OrderBy property name matches a valid property on the Jobs object, either by using a list of properties by string or reflection.

Things to think about for your own API: does case sensitivity matter? Do you want to allow them to select only certain properties to sort/filter by?

### Refactor your request handler

1. Change your request handler to order by the value of the OrderBy property if the OrderBy property on the request is not null. (A convenient extension method has been provided in the project that takes in a string and creates a custom expression for you that will send the OrderBy to the data source.)

```csharp
if (!string.IsNullOrWhiteSpace(message.OrderBy))
{
    jobs = jobs.OrderBy(message.OrderBy);
}
```

2. Change your request handler to filter values by number if the value of the Number property in the request 

### Test your changes manually

1. Fire up Postman and experiment with your new HTTP API. Execute GET /jobs with query parameters like so: `/jobs?orderBy=Name&number=123`

### Write new tests

1. Run your tests - they should pass.
2. Create new tests using the same pattern as before, but test your new query parameters in separate tests. Ensure that: 
- The order is what is expected when passing in an orderBy parameter.
- The request sends back 400 Bad Request when specifying an invalid orderBy parameter (hint: you can use reflection to check for this).
- The results are properly filtered when passing in a number parameter.