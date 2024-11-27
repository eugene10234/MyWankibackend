using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {private readonly WealthierAndKinderContext _context;
    private readonly ILogger<ChatHub> _logger;  // 添加 logger

    public ChatHub(
        WealthierAndKinderContext context,
        ILogger<ChatHub> logger)  // 注入 logger
    {
        _context = context;
        _logger = logger;
    }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var memberId = Context.User.FindFirst("MemberId")?.Value;

            if (string.IsNullOrEmpty(memberId))
            {
                throw new HubException("未授權的連接");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, memberId);
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string receiverId, string message)
        {
            try
            {
                var senderId = Context.User.FindFirst("MemberId")?.Value;
                if (string.IsNullOrEmpty(senderId))
                {
                    throw new HubException("未授權的操作");
                }

                var newMessage = new TMessage
                {
                    FSid = senderId,
                    FRid = receiverId,
                    FMessContent = message,
                    FCreateTime = DateTime.UtcNow,
                    FTimestamp = DateTime.UtcNow,
                    FMessType = "TEXT",
                    FIsRead = false
                };

                _context.TMessages.Add(newMessage);
                await _context.SaveChangesAsync();

                var messageData = new
                {
                    fMessId = newMessage.FMessid,
                    fMemberId = senderId,
                    fSid = senderId,
                    fRid = receiverId,
                    fMessContent = message,
                    fTimestamp = newMessage.FTimestamp,
                    fCreateTime = newMessage.FCreateTime,
                    fMessType = newMessage.FMessType,
                    fIsRead = newMessage.FIsRead,
                    fImagePath = newMessage.FImagePath,
                    fImageName = newMessage.FImageName
                };

                // 確保兩端都收到訊息
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", messageData);
                await Clients.Group(senderId).SendAsync("ReceiveMessage", messageData);

                // 記錄發送狀態
                _logger.LogInformation($"訊息已發送: 從 {senderId} 到 {receiverId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"發送訊息時發生錯誤: {ex.Message}");
                throw;
            }
        }
    }
}
