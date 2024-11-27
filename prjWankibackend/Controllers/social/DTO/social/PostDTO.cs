
    public class PostDTO
    {
        public int FPostId { get; set; }
        public string FMemberId { get; set; }
        public string FUserName { get; set; }
        public string FPostContent { get; set; }
        public string FMemberType { get; set; }
        public int FLikes { get; set; }
        // 修改為 string 類型以匹配前端
        public string FCreatedTime { get; set; }
        public bool IsLiked { get; set; }
        public string UserAvatar { get; set; } = "../assets/images/default-avatar.png";
        public int Comments { get; set; }
        public List<prjWankibackend.DTO.social.CommentDTO> CommentList { get; set; } = new List<prjWankibackend.DTO.social.CommentDTO>();
        public int? Reposts { get; set; }
    }


    

