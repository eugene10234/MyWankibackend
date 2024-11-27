namespace prjWankibackend.Controllers.social.DTO.social
{
    public class MessageDTO
    {
        public int FMessId { get; set; }
        public string FSId { get; set; }
        public string FRId { get; set; }
        public string FMessContent { get; set; }
        public DateTime FTimestamp { get; set; }
        public string FImagePath { get; set; }
        public string FImageName { get; set; }
        public string FMessType { get; set; }
        public bool FIsRead { get; set; }
        public DateTime FCreateTime { get; set; }
        public DateTime? FUpdateTime { get; set; }
    }
}
