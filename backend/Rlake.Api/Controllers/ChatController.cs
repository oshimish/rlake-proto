using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI.GPT3.Interfaces;
using Rlake.Api.Data;
using Rlake.Api.Dto;
using Rlake.Api.Services;

namespace Rlake.Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        public ChatController(ChatService chatService, 
            ApiDbContext dbContext,
            ILogger<ChatController> logger)
        {
            ChatService = chatService;
            DbContext = dbContext;
            Logger = logger;
        }

        public ChatService ChatService { get; }
        public ApiDbContext DbContext { get; }
        public ILogger Logger { get; }

        /// <summary>
        /// Creates a new AI conversation.
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<SearchResultDto>> Start(string searchText)
        {
            var result = await ChatService.Post(searchText);

            var conversation = new Conversation()
            {
                Title = result.SearchText,

            };
            DbContext.Add(conversation);

            var post = new Post()
            {
                Text = result.SearchText,
                Points = result.Items
            };

            conversation.Posts.Add(post);
            await DbContext.SaveChangesAsync();

            result.Conversation = conversation;

            return Ok(result);
        }

        /// <summary>
        /// Get last conversations list.
        /// </summary>
        [HttpGet()]
        public async Task<ActionResult<IList<Conversation>>> GetConversations()
        {
            var data = await DbContext.Conversations
                .Take(20)
                .Include(x => x.Posts).ThenInclude(x => x.Points)
                .ToListAsync();            

            return Ok(data);
        }

        /// <summary>
        /// Get conversation by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> GetConversationById([Required] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("empty id");
            }

            var conversation = await DbContext.LoadConversation(id);
            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(conversation);
        }

        /// <summary>
        /// Posts new message to the conversation.
        /// </summary>
        [HttpPost("{id}")]
        public async Task<ActionResult<Post>> PostToConversation([Required] Guid id, Post post)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("empty id");
            }

            var conversation = await DbContext.LoadConversation(id);
            if (conversation == null)
            {
                return NotFound();
            }

            post.ConversationId = id;
            conversation.Posts.Add(post);
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(post);
        }

        private bool ConversationExists(Guid id)
        {
            return (DbContext.Conversations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}