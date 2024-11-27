namespace prjWankibackend.DTO.help
{
    public class HelpDTO
    {
        public int HelpId { get; set; }
        
        public string Name { get; set; }

        
        public string Email { get; set; }

        
        public string Phone { get; set; }

        
        public int HelpClass { get; set; }

        
        public string HelpContent { get; set; }

        
        public int Status { get; set; }

        
        public DateTime CreateTime { get; set; }
        public int Points { get; set; }
        public int DistrictId { get; set; }

        public double? Latitude { get; set; } // 使用 double 類型來存儲緯度
        public double? Longitude { get; set; } // 使用 double 類型來存儲經度
        
    }
}
