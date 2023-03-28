using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Interfaces;
using Rlake.Api.Dto;
using Rlake.Api.Services;

namespace Rlake.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        public ChatController(ChatService chatService, ILogger<ChatController> logger)
        {
            ChatService = chatService;
            Logger = logger;
        }

        public ChatService ChatService { get; }
        public ILogger Logger { get; }

        [HttpPost]
        public async Task<ActionResult<SearchResult>> Search(string searchText)
        {
            var result = await ChatService.Post(searchText);

            return Ok(result);
        }
    }
}