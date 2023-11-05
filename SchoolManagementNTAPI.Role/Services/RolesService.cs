using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Authentication.Claims;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Role.Interfaces;
using SchoolManagementNTAPI.Role.Models;
using SchoolManagementNTAPI.Role.Requests;
using SchoolManagementNTAPI.Role.Responses;
using System.Security.Claims;
using System.Transactions;

namespace SchoolManagementNTAPI.Role.Services
{
    public class RolesService : IRolesService
    {
        private readonly AspNetRoleManager<IdentityRole> _roleManager;
        private readonly AspNetUserManager<IdentityUser> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public RolesService(AspNetRoleManager<IdentityRole> roleManager, AspNetUserManager<IdentityUser> userManager, IRefreshTokenService refreshTokenService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<OperationStatusResponse> CreateRole(CreateRoleRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (request.Name.Trim() == string.Empty)
                        return new OperationStatusResponse { Message = "Невалидно име." };

                    if (await _roleManager.RoleExistsAsync(request.Name))
                        return new OperationStatusResponse { Message = "Улогата веќе постои!" };

                    IdentityRole role = new(request.Name);

                    var result = await _roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                        return new OperationStatusResponse { Message = $"Server error: {result}" };

                    foreach (var claim in request.Claims)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(ClaimPermissionType.Permission, claim));
                    }

                    scope.Complete();

                    return new OperationStatusResponse();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new OperationStatusResponse { Message = $"Exception error: {ex.Message}" };
                }
            }
        }

        public async Task<OperationStatusResponse> UpdateRole(UpdateRoleRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(request.Id);

                    if (role is null)
                        return new OperationStatusResponse { Message = "Улогата не беше најдена!" };

                    if (role.Name != request.Name)
                    {
                        role.Name = request.Name;
                        var result = await _roleManager.UpdateAsync(role);
                        if (!result.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {result}" };
                    }

                    var claims = await _roleManager.GetClaimsAsync(role);

                    foreach (var item in claims)
                    {
                        if (!request.Claims.Any(x => x == item.Value))
                        {
                            await _roleManager.RemoveClaimAsync(role, item);
                        }
                    }

                    foreach (string claim in request.Claims)
                    {
                        if (!claims.Any(x => x.Value == claim))
                        {
                            await _roleManager.AddClaimAsync(role, new Claim(ClaimPermissionType.Permission, claim));
                        }
                    }

                    var userWithThatRole = await _userManager.GetUsersInRoleAsync(request.Name);
                    foreach (var user in userWithThatRole)
                    {
                        await _refreshTokenService.DeleteRefreshTokensForUser(user.Id);
                    }

                    scope.Complete();

                    return new OperationStatusResponse();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new OperationStatusResponse { Message = $"Exception error: {ex.Message}" };
                }
            }
        }

        public async Task<GetRolesResponse> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleModel
            {
                Id = x.Id,
                Name = x.Name ?? ""
            }).ToListAsync();

            return new GetRolesResponse() { Lista = roles };
        }

        public async Task<GetRoleByIdResponse> GetRoleById(IdRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);

            if (role is null)
                return new GetRoleByIdResponse { Message = "Улогата не беше најдена!" };

            var claims = await _roleManager.GetClaimsAsync(role);
            var claimsForRole = claims.Select(x => x.Value).ToList();

            return new GetRoleByIdResponse()
            {
                Name = role.Name ?? string.Empty,
                ClaimsForRole = claimsForRole,
            };
        }

        public async Task<OperationStatusResponse> DeleteRole(IdRoleRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(request.Id);

                    if (role is null)
                        return new OperationStatusResponse { Message = "Улогата не беше најдена!" };

                    // Proverka dali ima useri so taa rolja. Ako ima, nema brisenje
                    if (role.Name != null)
                    {
                        var listaNaUseriSoTaaUloga = await _userManager.GetUsersInRoleAsync(role.Name);
                        if (listaNaUseriSoTaaUloga.Count > 0)
                            return new OperationStatusResponse { Message = "Неможе да се избрише улогата бидејќи имате корисници со таа улога." };

                        var result = await _roleManager.DeleteAsync(role);
                        if (!result.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {result}" };

                        scope.Complete();
                        return new OperationStatusResponse();
                    }

                    scope.Complete();

                    return new OperationStatusResponse { Message = "Server error" };
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new OperationStatusResponse { Message = $"Exception error: {ex.Message}" };
                }
            }
        }

        public GetAllClaimsResponse GetAllClaims()
        {
            return new GetAllClaimsResponse() { Lista = Enum.GetNames(typeof(UserClaims)).ToList() };
        }

        public async Task<GetRolesDropDownResponse> GetRolesDropDown()
        {
            var dropdown = await _roleManager.Roles.Select(x => new NameOfRole
            {
                Name = x.Name ?? ""
            }).ToListAsync();

            return new GetRolesDropDownResponse() { Lista = dropdown };
        }
    }
}
