namespace prjWankibackend.DTO.social
{
public class CommentDTO
{
    public int FCommentId { get; set; }
    public int FPostId { get; set; }
    public string FMemberId { get; set; }
    public string FContent { get; set; }
    public string FCratedAT { get; set; }
    public string FUpdateAt { get; set; }

}
}
