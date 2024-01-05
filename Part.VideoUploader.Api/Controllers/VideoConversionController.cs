
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Part.VideoUploader.Service.Features.VideoConversion.Commands;
using Part.VideoUploader.Service.Responses;
using IMediator = MediatR.IMediator;

namespace Part.VideoUploader.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoConversionController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideoConversionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("convert")]
    public async Task<ActionResult<BaseResponse>> Convert([FromForm] ConvertVideoCommand request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = userId;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User must be logged in to request video conversion.");
        }

        if (string.IsNullOrEmpty(request.FileName) || request.EncodingParameters == null)
        {
            return BadRequest("Invalid request parameters for video conversion.");
        }

        try
        {
            var response = await _mediator.Send(new ConvertVideoCommand(userId, request.FileName, request.EncodingParameters));
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred during the video conversion process: {ex.Message}");
        }
    }
}


