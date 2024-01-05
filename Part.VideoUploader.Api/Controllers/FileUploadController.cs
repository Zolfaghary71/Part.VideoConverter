using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Part.VideoUploader.Service.Features.Storage.Commands;
using Part.VideoUploader.Service.Responses;
using IMediator = MediatR.IMediator;

namespace Part.VideoUploader.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<BaseResponse>> Upload()
    {
        if (HttpContext.Request.Form.Files.Count == 0 || HttpContext.Request.Form.Files[0].Length == 0)
        {
            return BadRequest("No file received or file is empty.");
        }

        var file = HttpContext.Request.Form.Files[0];
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User must be logged in to upload files.");
        }

        var fileName = file.FileName;
        var contentType = file.ContentType;

        try
        {
            using (var stream = file.OpenReadStream())
            {
                var response = await _mediator.Send(new UploadFileCommand(stream, fileName, contentType, userId));
                return Ok(response);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while uploading the file: {ex.Message}");
        }
    }
}
