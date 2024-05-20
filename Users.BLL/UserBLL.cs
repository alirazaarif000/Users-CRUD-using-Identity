using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Users.Entities.Models;

namespace Users.BLL
{
    public class UserBLL
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserBLL(UserManager<IdentityUser> userManger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManger;
            _roleManager = roleManager;
        }
        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }
        public async Task<List<IdentityUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<IdentityResult> CreateUser(Register model)
        {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded && !string.IsNullOrEmpty(model.Role))
            {
                result = await _userManager.AddToRoleAsync(user, model.Role);
            }
            return result;
        }

        public async Task<Register> GetUserForEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            var UserRole = await _userManager.GetRolesAsync(user);
            var Roles= await GetAllRolesAsync();
            return new Register
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = UserRole.FirstOrDefault(),
                RoleList = Roles
            }; 
        }

        public async Task<IdentityResult> UpdateUser(Register model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User Not Found" });
            user.UserName = model.Username;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var roleResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!roleResult.Succeeded) return roleResult;
                if (!string.IsNullOrEmpty(model.Role))
                {
                     roleResult= await _userManager.AddToRoleAsync(user, model.Role);
                }
                return roleResult;
            }
            return result;
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.DeleteAsync(user);
        }

        public async Task<Register> GetUserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            return new Register
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                RoleList = userRoles
            };
        }
        
    }
}