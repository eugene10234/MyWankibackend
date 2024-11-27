namespace prjWankibackend.Controllers.Member.DTO
{
    public class ProfileUpdateDto
    {
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public DateTime birthDate { get; set; }
        public int districtId { get; set; }
    }
    public class ProfileResponseDto
    {
        public string UserAccount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } // 如果需要區域名稱
    }

}
