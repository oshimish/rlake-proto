
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RlakeFunctionApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggerAdapter<NotificationService> _logger;

        public NotificationService(HttpClient httpClient, ILoggerAdapter<NotificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendConversationStartedEventAsync(CreateConversationOptions createNoteOptions, CancellationToken ct)
        {
            try
            {
                var noteCreatedRequest = new ConversationCreatedRequest
                {
                    Title = createNoteOptions.SearchText,
                    Description = createNoteOptions.SearchText
                };
                var json = JsonSerializer.Serialize(noteCreatedRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponseMessage = await _httpClient.PostAsync("notes", data, ct);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseContentStr = await httpResponseMessage.Content.ReadAsStringAsync(ct);

                    _logger.LogError(responseContentStr);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(NotificationService)} -> {nameof(SendConversationStartedEventAsync)} method.");
            }
        }
    }
}
