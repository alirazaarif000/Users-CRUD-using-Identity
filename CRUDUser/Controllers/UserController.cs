using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Users.BLL;

namespace CRUDUser.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class UserController : Controller
    {
        private readonly UserBLL _userBLL;
        public UserController(UserBLL userBLL)
        {
            _userBLL= userBLL;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("User/Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _userBLL.GetAllRolesAsync();
            var model = new Register
            {
                RoleList = roles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Register model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleList = await _userBLL.GetAllRolesAsync();
                return View(model);
            }
            var result = await _userBLL.CreateUser(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            model.RoleList = await _userBLL.GetAllRolesAsync();
            return View(model);
        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
           var model= await _userBLL.GetUserForEdit(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Register model)
        {
             var result= await _userBLL.UpdateUser(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.RoleList = await _userBLL.GetAllRolesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userBLL.DeleteUser(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Index", await _userBLL.GetAllUsers());
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var model = await _userBLL.GetUserDetails(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _userBLL.GetAllUsers();
            return Json(Users);
        }
    }
}
