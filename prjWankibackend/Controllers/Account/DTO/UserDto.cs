namespace prjWankibackend.Controllers.Account.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Provider { get; set; } // "Google" or "Local"
        public DateTime CreatedAt { get; set; }

    }
}
