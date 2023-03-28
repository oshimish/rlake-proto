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
        public async Task<ActionResult<SearchResultDto>> Search(string searchText)
        {
            var result = await ChatService.Post(searchText);

            return Ok(result);
        }

        [HttpGet("conversations")]
        public IEnumerable<Conversation> GetConversations()
        {
            return Enumerable.Range(1, 5).Select(index => new Conversation
            {
                Id = Guid.NewGuid(),
                Title = $"Point {index}"
            })
            .ToArray();
        }

        [HttpGet("conversations/{id}")]
        public ActionResult<Conversation> GetConversationById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var loc = new Conversation()
            {
                Id = id,
                Title = $"Item {id}",
            };
            return Ok(loc);
        }

        [HttpPost("conversations/{id}")]
        public ActionResult<Post> PostToConversation(Guid id, Post post)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var loc = new Post()
            {
                Id = id,
                Text = $"Post {id}",
            };
            return Ok(loc);
        }
    }
}