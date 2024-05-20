using Users.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUDUser.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        public LoginController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View("Views//Login/Index.cshtml", model);
        }
    }


    //[HttpPost]
    //public async Task<IActionResult> Login(Login model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(model);
    //    }
    //    var user = await userManager.FindByNameAsync(model.Username);
    //    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
    //    {
    //        var userRoles = await userManager.GetRolesAsync(user);

    //        var authClaims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.Name, user.UserName),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //        };

    //        foreach (var userRole in userRoles)
    //        {
    //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
    //        }

    //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

    //        var token = new JwtSecurityToken(
    //            issuer: _configuration["JWT:ValidIssuer"],
    //            audience: _configuration["JWT:ValidAudience"],
    //            expires: DateTime.Now.AddHours(3),
    //            claims: authClaims,
    //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    //            );

    //        new JwtSecurityTokenHandler().WriteToken(token);

    //        return RedirectToAction("Index", "User");
    //    }
    //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //    return View(model);
    //}
}

