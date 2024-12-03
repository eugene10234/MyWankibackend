using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using System.Security.Claims;
using prjWankibackend.Models.Account.Jwt;
using prjWankibackend.Controllers.Member.DTO;

namespace prjWankibackend.Services.Account.Member
{
    public class MemberService : IMemberService
    {
        private readonly WealthierAndKinderContext _context;
        public MemberService(WealthierAndKinderContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateProfileAsync(string userId, ProfileUpdateDto updateDto)
        {
            var user = await _context.TPersonMembers
                .FirstOrDefaultAsync(u => u.FPersonSid.ToString() == userId);

            if (user == null)
            {
                return false;
            }

            // 更新用戶資料
            user.FFirstName = updateDto.firstName;
            user.FLastName = updateDto.lastName;
            user.FSex = updateDto.gender;
            user.FBirthDate = DateOnly.FromDateTime(updateDto.birthDate);
            user.FDistrictId = updateDto.districtId;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<ProfileResponseDto> GetProfileAsync(string userId)
        {
            try
            {
                var user = await _context.TPersonMembers
                    //.Include(u => u.District)  // 如果需要區域資訊
                    .FirstOrDefaultAsync(u => u.FPersonSid.ToString() == userId);

                if (user == null)
                {
                    //_logger.LogWarning("找不到用戶 ID: {UserId}", userId);
                    return null;
                }

                //_logger.LogInformation("成功獲取用戶資料 ID: {UserId}", userId);
                return new ProfileResponseDto
                {
                    UserAccount = user.FAccount,
                    FirstName = user.FFirstName,
                    LastName = user.FLastName,
                    Gender = user.FSex,
                    BirthDate = user.FBirthDate?.ToDateTime(TimeOnly.MinValue),
                    DistrictId = user.FDistrictId ?? 0,
                    // 可以加入其他需要的資料
                    //DistrictName = user.District?.Name // 如果需要區域名稱
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "獲取用戶資料時發生錯誤 UserId: {UserId}", userId);
                throw; // 將錯誤往上拋給控制器處理
            }
        }



    }
}
