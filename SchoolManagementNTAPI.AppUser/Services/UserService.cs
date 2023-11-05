using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.AppUser.Interfaces;
using SchoolManagementNTAPI.AppUser.Models;
using SchoolManagementNTAPI.AppUser.Requests;
using SchoolManagementNTAPI.AppUser.Responses;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Data.Entities;
using SchoolManagementNTAPI.Role.Models;
using System.Transactions;

namespace SchoolManagementNTAPI.AppUser.Services
{
    public class UserService : IUserService
    {
        private readonly AspNetUserManager<IdentityUser> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly SchoolManagementNTDBContext _context;
        private readonly AspNetRoleManager<IdentityRole> _roleManager;

        public UserService(AspNetUserManager<IdentityUser> userManager,
            IRefreshTokenService refreshTokenService,
            SchoolManagementNTDBContext context,
            AspNetRoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<OperationStatusResponse> CreateUser(CreateUserRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (request.Password != request.ConfirmPassword)
                        return new OperationStatusResponse { Message = "Лозинка и потврди лозинка не се совпаѓаат." };

                    var userToAdd = new IdentityUser
                    {
                        Email = request.Email.ToLower(),
                        UserName = request.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(userToAdd, request.Password);
                    if (!result.Succeeded)
                        return new OperationStatusResponse { Message = $"Server error: {result}" };

                    if (request.Role is not null)
                    {
                        var resultFromRoleAssigment = await _userManager.AddToRoleAsync(userToAdd, request.Role);
                        if (!resultFromRoleAssigment.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {resultFromRoleAssigment}" };
                    }

                    scope.Complete();

                    return new OperationStatusResponse { Message = "Ok." };
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new OperationStatusResponse { Message = $"Exception error: {ex.Message}" };
                }
            }
        }

        public async Task<OperationStatusResponse> Update(UpdateUserRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(request.Id);

                    if (user == null)
                        return new OperationStatusResponse { Message = $"Не постои корисникот." };

                    if (request.Email.Trim() == "")
                    {
                        return new OperationStatusResponse { Message = $"Внесете валиден email." };
                    }

                    if (user.Email != request.Email)
                    {
                        var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
                        var result = await _userManager.ChangeEmailAsync(user, request.Email, token);

                        if (!result.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {result}" };
                    }

                    if (user.LockoutEnabled != request.LockoutEnabled)
                    {
                        var result = await _userManager.SetLockoutEnabledAsync(user, request.LockoutEnabled);

                        if (!result.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {result}" };
                    }

                    await _refreshTokenService.DeleteRefreshTokensForUser(request.Id);

                    var currentRoles = await _userManager.GetRolesAsync(user);

                    if (request.Role == null)
                    {
                        var resultFromRemovingRoles = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        if (!resultFromRemovingRoles.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {resultFromRemovingRoles}" };

                        return new OperationStatusResponse();
                    }

                    if (!currentRoles.Contains(request.Role!))
                    {
                        var resultFromRemovingRoles = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        if (!resultFromRemovingRoles.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {resultFromRemovingRoles}" };

                        var resultFromRoleAssigment = await _userManager.AddToRoleAsync(user, request.Role!);
                        if (!resultFromRoleAssigment.Succeeded)
                            return new OperationStatusResponse { Message = $"Server error: {resultFromRoleAssigment}" };
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

        public async Task<ListOfUsersResponse> ListOfUsers()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            List<UserWithRoles> usersWithRoles = new();

            foreach (var item in roles)
            {
                if (item is not null && item.Name != "")
                {
                    UserWithRoles newUser = new()
                    {
                        RoleName = item.Name ?? "",
                        Users = await _userManager.GetUsersInRoleAsync(item.Name!)
                    };
                    usersWithRoles.Add(newUser);
                }
            }

            List<UserModel> users = new();

            foreach (var item in usersWithRoles)
            {
                var lista = item.Users?.Select(x => new UserModel
                {
                    Id = x.Id,
                    Email = x.Email ?? "",
                    LockoutEnd = x.LockoutEnd,
                    LockoutEnabled = x.LockoutEnabled,
                    AccessFailedCount = x.AccessFailedCount,
                    EmailConfirmed = x.EmailConfirmed,
                    Role = item.RoleName
                }).ToList();

                if (lista is not null)
                    users.AddRange(lista);
            }


            var distinctUserName = users.Select(x => x.Email).Distinct();

            var allUsers = await _userManager.Users.Select(x => new UserModel
            {
                Id = x.Id,
                Email = x.Email ?? "",
                LockoutEnd = x.LockoutEnd,
                LockoutEnabled = x.LockoutEnabled,
                AccessFailedCount = x.AccessFailedCount,
                EmailConfirmed = x.EmailConfirmed,
                Role = ""
            }).ToListAsync();

            foreach (var item in allUsers)
            {
                if (!distinctUserName.Contains(item.Email))
                {
                    users.Add(item);
                }
            }

            return new ListOfUsersResponse { Lista = users.OrderBy(x => x.Email).ToList() };
        }

        public async Task<GetUserByIdResponse> GetUserById(IdUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
                return new GetUserByIdResponse { Message = "User not found." };

            var userRoles = await _userManager.GetRolesAsync(user);

            RoleModel roleModel = new();

            if (userRoles.Count > 0)
            {
                var role = await _roleManager.FindByNameAsync(userRoles[0]);

                if (role is not null)
                {
                    roleModel.Id = role.Id ?? "";
                    roleModel.Name = role.Name ?? "";
                }
            }

            GetUserByIdResponse response = new()
            {
                Id = user.Id,
                Email = user.Email!,
                LockoutEnabled = user.LockoutEnabled,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnd = user.LockoutEnd,
                Role = roleModel.Name == "" ? null : roleModel.Name,
            };

            return response;
        }

        public async Task<OperationStatusResponse> ResetLockout(IdUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
                return new OperationStatusResponse { Message = $"Не постои корисникот." };

            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (!result.Succeeded)
                return new OperationStatusResponse { Message = $"Server error: {result}" };

            return new OperationStatusResponse();
        }

        public async Task<OperationStatusResponse> ToogleUser(IdUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
                return new OperationStatusResponse { Message = $"Не постои корисникот." };

            user.EmailConfirmed = !user.EmailConfirmed;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new OperationStatusResponse { Message = $"Server error: {result}" };

            await _refreshTokenService.DeleteRefreshTokensForUser(request.Id);

            return new OperationStatusResponse();
        }

        public async Task<OperationStatusResponse> ChangePassword(ChangePasswordRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return new OperationStatusResponse { Message = "Лозинка и потврди лозинка не се совпаѓаат." };

            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
                return new OperationStatusResponse { Message = $"Не постои корисникот." };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
                return new OperationStatusResponse { Message = $"Server error: {result}" };

            await _refreshTokenService.DeleteRefreshTokensForUser(request.Id);

            return new OperationStatusResponse();
        }

        public async Task<UserDropDownResponse> UsersDropDown()
        {
            var studentTableUserIds = await _context.Student
                .Select(x => x.AspNetUserId).ToListAsync();

            var filteredList = await _userManager.Users
                .Select(user => new UserDropDownItem
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Disabled = studentTableUserIds.Contains(user.Id)
                })
                .ToListAsync();

            return new UserDropDownResponse { Lista = filteredList };
        }
    }

    internal class UserWithRoles
    {
        public string RoleName { get; set; } = string.Empty;
        public IList<IdentityUser>? Users { get; set; }
    }
}
