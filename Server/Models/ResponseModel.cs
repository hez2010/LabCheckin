using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCheckin.Server.Models
{
    public abstract record ResponseModel<T>(string? Message, T? Data, int Code) : IActionResult
    {
        private readonly static JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task ExecuteResultAsync(ActionContext context)
        {
            switch (this)
            {
                case StreamResultModel s:
                    context.HttpContext.Response.StatusCode = Code;
                    if (s.FileName is not null)
                    {
                        context.HttpContext.Response.Headers
                            .Add("Content-Disposition", $"inline; filename=\"{Uri.EscapeUriString(s.FileName)}\"");
                    }
                    var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<FileStreamResult>>();
                    await executor.ExecuteAsync(context, new FileStreamResult(s.Stream, s.ContentType));
                    break;
                default:
                    context.HttpContext.Response.StatusCode = Code;
                    await JsonSerializer.SerializeAsync(context.HttpContext.Response.Body, new { Message, Data, Code }, options);
                    break;
            }
        }
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
        string ContentType = "application/octet-stream") : OkResponseModel<Stream>(default, Stream);
}
