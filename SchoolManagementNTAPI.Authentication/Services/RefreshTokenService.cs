using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Data.Entities;

namespace SchoolManagementNTAPI.Authentication.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly SchoolManagementNTDBContext _context;

        public RefreshTokenService(SchoolManagementNTDBContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteRefreshTokensForUser(string userId)
        {
            var tokens = await _context.RefreshToken.Where(x => x.AspNetUserId == userId).ToListAsync();
            foreach (var item in tokens)
            {
                _context.RefreshToken.Remove(item);
            }
            var rowsChanged = await _context.SaveChangesAsync();

            return rowsChanged;
        }

        public async Task<RefreshTokenDTO> SaveRefreshTokensForUser(RefreshTokenDTO refreshToken)
        {
            _context.RefreshToken.Add(
                                            new RefreshToken
                                            {
                                                IsValid = true,
                                                TokenHash = refreshToken.TokenHash,
                                                AspNetUserId = refreshToken.AspNetUserId
                                            });

            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
