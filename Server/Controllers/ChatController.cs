using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetroChat.Server.Services;
using RetroChat.Shared.Models;
using System;
using System.Diagnostics;

namespace RetroChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatService _chatService;

        public ChatController(ILogger<ChatController> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpGet]
        [Route("backlog")]
        public IActionResult FetchBacklog()
        {
            return Ok(_chatService.FetchBacklog());
        }

        [HttpPost]
        [Route("availability")]
        public IActionResult Availability([FromBody] string handle)
        {
            return Ok(!_chatService.UserIsConnected(handle));
        }
    }
}