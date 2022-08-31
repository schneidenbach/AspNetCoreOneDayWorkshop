using System;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs.UpdateJob
{
    public class UpdateJobRequest : IRequest<Func<ControllerBase, IActionResult>>
    {
        public UpdateJobRequest(int id, JsonElement data)
        {
            Id = id;
            Data = data;
        }
        
        public int Id { get; }
        public JsonElement Data { get; }
    }
}
