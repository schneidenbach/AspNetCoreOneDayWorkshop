# First CQRS implementation in ASP.NET Core

The goal of this lab is to use the concepts we've learned about so far to create a tiny API that uses all of the parts we've already talked about!

## Concepts

- Learn the CQRS pattern.
- Creating request objects that will be your entry point for HTTP requests. 
- Validating the requests using FluentValidation.
- Writing handlers to handle the request's intent and return data to the caller.
- Write the response object.

## Tasks

### Create the request

1. Add a class to the root of the project called HelloWorldRequest. This will be the entry point to your API.
2. Add the `IRequest<string>` interface to the class.
3. Add a string property called Message to the class.

### Create the validator 

1. Add a class to the root of the project called HelloWorldRequestValidator. This will build the class that will describe how to validate the HelloWorldRequest object.
2. Inherit from the `AbstractValidator<HelloWorldRequest>` class.
3. Add a constructor and add a validator rule to make sure that the Message property is not null or an empty string:

```csharp
RuleFor(r => r.Message).NotEmpty().WithMessage("Please enter a message.");
```

### Create the request handler

1. Add a class to the root of the project called HelloWorldRequestHandler. This will execute code that uses the request to perform an action. In this case, we're just going to use it to return a string that will be a message back to the client.
2. Implement the `IRequestHandler<HelloWorldRequest, string>` interface.
3. Use the tooling to auto-add the Handle method that will be executed.
4. Return `"Hello, " + request.Message` from the Handle method. (You may have to use Task.FromResult or declare the method as async.)

### Add the HTTP method to the `WorkshopController`

1. Add a method to the `WorkshopController` that takes in the request and passes it to HandleRequestAsync, like so:

```csharp
public Task<IActionResult> TestMethod(HelloWorldRequest request)
{
    return HandleRequestAsync(request);
}
```

2. Add the `HttpPost` attribute to the method and pass in `"helloworld"` as an argument.

### Test your method out

1. Fire up your API project using your favorite tool or the command line: `dotnet run`
2. Wherever you're running it from, note the port number of the application that was started (will usually be 8000).
3. Open up Postman and create a POST request to: `http://localhost:8000/api/workshop/helloworld`
4. Change the request body to be `application/json` and pass in an object like so:
```json
{
    "message": "your_name_here"
}
```
5. Try the request again multiple ways - no content, no value in the `message` property, etc.

BONUS: Experiment with FluentValidation - explore its API a little more and see what other validation patterns you can use with it.