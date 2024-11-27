using prjWankibackend.Controllers.Member.DTO;
using System.Threading.Tasks;

namespace prjWankibackend.Controllers.Member.Services.Member
{
    public interface IMemberService
    {
        Task<bool> UpdateProfileAsync(string userId, ProfileUpdateDto updateDto);
        Task<ProfileResponseDto> GetProfileAsync(string userId);
    }
}
