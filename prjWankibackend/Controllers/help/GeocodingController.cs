using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjWankibackend.DTO.help;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Controllers.help
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeocodingController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly WealthierAndKinderContext _context;


        public GeocodingController(HttpClient httpClient, WealthierAndKinderContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        //將地址轉譯為經緯度的方法
        [HttpGet("get-coordinates")]
        public async Task<IActionResult> GetCoordinates([FromQuery] string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("地址不能為空");
            }

            try
            {
                // 替換為你的 Google Maps API Key
                string apiKey = "AIzaSyAVCDcCRj_MRVQmZ9a2afJF8AM1MIdFmLc";

                string encodedAddress = Uri.EscapeDataString(address);
                string requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={apiKey}";

                // 發送 HTTP GET 請求到 Google Geocoding API
                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "無法從 Google Geocoding API 獲取結果");
                }

                // 解析 API 回應結果
                var responseBody = await response.Content.ReadAsStringAsync();
                var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(responseBody);

                // 添加日誌以檢查回應內容
                Console.WriteLine(responseBody);

                // 檢查 API 回應狀態
                if (geocodingResponse.Status == "OK" && geocodingResponse.Results.Length > 0)
                {
                    var location = geocodingResponse.Results[0].Geometry.Location;
                    return Ok(new { Latitude = location.Lat, Longitude = location.Lng });
                }
                else
                {
                    // 添加日誌以檢查回應狀態
                    Console.WriteLine($"Geocoding API Status: {geocodingResponse.Status}");
                    if (geocodingResponse.Results.Length == 0)
                    {
                        Console.WriteLine("No results found.");
                    }
                    else
                    {
                        Console.WriteLine("Unexpected response format.");
                    }
                }

                return NotFound("無法找到對應的經緯度資訊");
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"外部 API 請求失敗：{e.Message}");
            }
        }
        public class GeocodingResponse
        {
            public string Status { get; set; }
            public GeocodingResult[] Results { get; set; }
        }

        public class GeocodingResult
        {
            public Geometry Geometry { get; set; }
        }

        public class Geometry
        {
            public Location Location { get; set; }
        }

        public class Location
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }



      
       

        [HttpGet("getCoordinatesByHelpClassId")]
        public async Task<IActionResult> GetCoordinatesByHelpClassId([FromQuery] int helpClassId)
        {

            if (helpClassId <= 0)
            {
                return BadRequest("helpClassId 不能為空或小於等於零");
            }

            var results = await _context.THelps
                .Where(t => t.FHelpClassId == helpClassId &&t.FHelpStatus==1)
                .Select(t=> new HelpDTO 
                {
                    Name = t.FName,
                    HelpClass = t.FHelpClassId ?? 0,
                    DistrictId = t.FDistrictId ?? 0,
                    Latitude = t.FLatitude,
                    Longitude = t.FLongitude,
                    CreateTime=t.FMfdDate ?? DateTime.Now,
                    HelpContent=t.FHelpDescribe,
                    Points=t.FPoint ?? 0,
                    HelpId=t.FHelpId,
                    
                  
                })
                .ToListAsync();

            if (results == null || results.Count == 0)
            {
                return NotFound("無法找到對應的經緯度資訊");
            }

            return Ok(results);
        }
    }
    
}
