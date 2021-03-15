using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCenter.Server.Models
{
    public abstract record ResponseModel<T>(string? Message, T? Data, int Code) : IActionResult
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

        public static implicit operator ResponseModel<T>(T? data) => new JsonResultModel<T>(data);
    }

    public abstract record OkResponseModel<T>(string? Message, T? Data) : ResponseModel<T>(Message, Data, 200);
    public abstract record ErrorResponseModel<T>(
        string? Message, int Code, T? Data = default) : ResponseModel<T>(Message, Data, Code);

    public record NotFoundModel<T>(string? Message = default) : ErrorResponseModel<T>(Message, 404);
    public record UnauthorizedModel<T>(string? Message = default) : ErrorResponseModel<T>(Message, 401);
    public record ForbiddenModel<T>(string? Message = default) : ErrorResponseModel<T>(Message, 403);
    public record BadRequestModel<T>(string? Message = default, T? Data = default) : ErrorResponseModel<T>(Message, 400, Data);
    public record JsonResultModel<T>(T? Data, string? Message = default) : OkResponseModel<T>(Message, Data);
    public record StreamResultModel(Stream Stream, string? FileName = default,
        string ContentType = "application/octet-stream") : OkResponseModel<Stream>(default, Stream)
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
            await executor.ExecuteAsync(context, new FileStreamResult(Stream, ContentType));
        }
    }
}
