using prjWankibackend.Controllers.Member.DTO;
using System.Threading.Tasks;

namespace prjWankibackend.Services.Account.Member
{
    public interface IMemberService
    {
        Task<bool> UpdateProfileAsync(string userId, ProfileUpdateDto updateDto);
        Task<ProfileResponseDto> GetProfileAsync(string userId);
    }
}
