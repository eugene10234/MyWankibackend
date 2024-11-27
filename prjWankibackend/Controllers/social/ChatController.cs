using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using prjWankibackend.Models.Database;
using prjWankibackend.Models.Account.Jwt;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly WealthierAndKinderContext _context;
    private readonly JwtHelper _jwtHelper;

    public ChatController(WealthierAndKinderContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    [HttpGet("history/{memberId}")]
    public async Task<IActionResult> GetChatHistory(string memberId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var currentUserId = User.FindFirst("MemberId")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var messages = await _context.TMessages
                .Where(m =>
                    (m.FSid == currentUserId && m.FRid == memberId) ||
                    (m.FSid == memberId && m.FRid == currentUserId))
                .OrderByDescending(m => m.FCreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new
                {
                    FMessId = m.FMessid,
                    FMemberId = m.FSid,  // 發送者ID
                    FSId = m.FSid,       // 發送者ID
                    FRId = m.FRid,       // 接收者ID
                    FMessContent = m.FMessContent,
                    FTimestamp = m.FTimestamp,
                    FImagePath = m.FImagePath,
                    FImageName = m.FImageName,
                    FMessType = m.FMessType ?? "TEXT",
                    FIsRead = m.FIsRead,
                    FCreateTime = m.FCreateTime
                })
                .ToListAsync();

            return Ok(new
            {
                messages = messages.OrderBy(m => m.FCreateTime),
                totalCount = await _context.TMessages
                    .Where(m =>
                        (m.FSid == currentUserId && m.FRid == memberId) ||
                        (m.FSid == memberId && m.FRid == currentUserId))
                    .CountAsync()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    [HttpGet("list/{memberId}")]
    public async Task<IActionResult> GetChatList(string memberId)
    {
        try
        {
            // ���ˬd�Τ�O�_�s�b
            var user = await _context.TPersonMembers
                .FirstOrDefaultAsync(m => m.FMemberId == memberId);

            if (user == null)
            {
                return NotFound($"�䤣��Τ� {memberId}");
            }

            var chatPartners = await _context.TMessages
                .Where(m => m.FSid == memberId || m.FRid == memberId)
                .Select(m => m.FSid == memberId ? m.FRid : m.FSid)
                .Distinct()
                .ToListAsync();

            var chatList = new List<object>();
            foreach (var partnerId in chatPartners)
            {
                var partner = await _context.TPersonMembers
                    .FirstOrDefaultAsync(p => p.FMemberId == partnerId);

                if (partner != null)
                {
                    var lastMessage = await _context.TMessages
                        .Where(m => (m.FSid == memberId && m.FRid == partnerId) ||
                                  (m.FSid == partnerId && m.FRid == memberId))
                        .OrderByDescending(m => m.FCreateTime)
                        .FirstOrDefaultAsync();

                    chatList.Add(new
                    {
                        id = partner.FMemberId,
                        name = partner.FUserName,
                        avatar = partner.FMemberImagePath,
                        lastMessage = lastMessage?.FMessContent,
                        lastMessageTime = lastMessage?.FCreateTime
                    });
                }
            }

            return Ok(chatList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, statusCode = 500 });
        }
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        try
        {
            var senderId = User.FindFirst("MemberId")?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                return Unauthorized();
            }

            // 建立新訊息
            var message = new TMessage
            {
                FSid = senderId,
                FRid = request.ReceiverId,
                FMessContent = request.Content,
                FCreateTime = DateTime.UtcNow,
                FTimestamp = DateTime.UtcNow,
                FMessType = "TEXT",
                FIsRead = false
            };

            _context.TMessages.Add(message);
            await _context.SaveChangesAsync();

            // 回傳訊息資料
            return Ok(new
            {
                FMessId = message.FMessid,
                FMemberId = senderId,
                FSId = senderId,
                FRId = request.ReceiverId,
                FMessContent = message.FMessContent,
                FTimestamp = message.FTimestamp,
                FCreateTime = message.FCreateTime,
                FMessType = message.FMessType,
                FIsRead = message.FIsRead,
                FImagePath = message.FImagePath,
                FImageName = message.FImageName
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // 添加請求模型
    public class SendMessageRequest
    {
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }

}

