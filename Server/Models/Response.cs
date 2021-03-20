using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCenter.Server.Models
{
    public abstract record Response<T>(string? Message, T? Data, int Code) : IActionResult
    {
        protected readonly static JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = Code;
            await JsonSerializer.SerializeAsync(context.HttpContext.Response.Body, new { Message, Data, Code }, options);
        }

        public static implicit operator Response<T>(T? data) => new Response<T>.Ok.Json(data);


        public abstract record Ok(string? Message, T? Data) : Response<T>(Message, Data, 200)
        {
            public record Json(T? Data, string? Message = default) : Ok(Message, Data);
            public record Stream(System.IO.Stream Content, string? FileName = default,
                string ContentType = "application/octet-stream") : Ok(default, default)
            {
                public override async Task ExecuteResultAsync(ActionContext context)
                {
                    context.HttpContext.Response.StatusCode = Code;
                    if (FileName is not null)
                    {
                        context.HttpContext.Response.Headers
                            .Add("Content-Disposition", $"inline; filename=\"{Uri.EscapeDataString(FileName)}\"");
                    }

                    var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<FileStreamResult>>();
                    await executor.ExecuteAsync(context, new FileStreamResult(Content, ContentType));
                }
            }
        }

        public abstract record Error(
            string? Message, int Code, T? Data = default) : Response<T>(Message, Data, Code)
        {
            public record NotFound(string? Message = default) : Error(Message, 404);
            public record Unauthorized(string? Message = default) : Error(Message, 401);
            public record Forbidden(string? Message = default) : Error(Message, 403);
            public record BadRequest(string? Message = default, T? Data = default) : Error(Message, 400, Data);
        }
    }
}
