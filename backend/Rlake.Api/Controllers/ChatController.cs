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
            ILogger<ChatController> logger,
            IMapper mapper)
        {
            ChatService = chatService;
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        public ChatService ChatService { get; }
        public ApiDbContext DbContext { get; }
        public ILogger Logger { get; }
        public IMapper Mapper { get; }

        /// <summary>
        /// Creates a new AI conversation.
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<SearchResultDto>> Start(string searchText)
        {
            var completion = await ChatService.Post(searchText);
            var result = Mapper.Map<SearchResultDto>(completion);

            var conversation = new Conversation()
            {
                Title = result.SearchText,
            };

            var post = new Post()
            {
                Text = result.SearchText,
                Points = result.Items
            };
            conversation.Posts.Add(post);

            // hack: store only if any locations
            if (completion.HasData && completion.Locations.Any())
            {
                DbContext.Add(post);
                DbContext.Add(conversation);
                await DbContext.SaveChangesAsync();
            }
            else
            {
                conversation.Id = Guid.NewGuid();
                post.Id = Guid.NewGuid();
                post.ConversationId = conversation.Id;
            }

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
                .Where(x => x.Posts.SelectMany(p => p.Points).Any())
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