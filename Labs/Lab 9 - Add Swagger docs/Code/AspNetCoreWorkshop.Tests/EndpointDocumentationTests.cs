using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetCoreWorkshop.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class EndpointDocumentationTests : TestBase
    {
        [Test]
        public void All_endpoints_contain_at_least_one_response_type_descriptor()
        {
            var controllerMethods = typeof(Startup)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ControllerBase)))
                .SelectMany(t => t.GetMethods().Where(m => m.GetCustomAttributes<HttpMethodAttribute>().Any()))
                .ToArray();
            
            var errors = new List<string>();
            foreach (var method in controllerMethods)
            {
                var contentDescriptionAttributes = method.GetCustomAttributes<ProducesResponseTypeAttribute>();
                if (!contentDescriptionAttributes.Any())
                {
                    var action = method.DeclaringType.Name + "." + method.Name;
                    errors.Add($"Missing response content descriptor for {action} - please add at least one [ProducesResponseType] attribute.");
                }
            }

            if (errors.Any())
            {
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }
    }
}