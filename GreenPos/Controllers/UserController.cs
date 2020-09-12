using Microsoft.AspNetCore.Mvc;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using System.Threading.Tasks;
using System.Linq;

namespace GreenPOS.Controllers
{
    public class UserController : BaseController
    {
        #region Properties
        public readonly IUserService _service;
        public readonly IRoleService _roleService;
        #endregion

        #region Constructors
        public UserController(IUserService service,IRoleService roleservice)
        {
            _service = service;
            _roleService = roleservice;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetUsersAsync(UserId);

            ViewBag.users = result;

            return View();

            //return View(result.ToList());
        }

        [HttpGet]
        [Route("User/GetUserAsync/{id}")]
        public async Task<IActionResult> GetUserAsync(long id)
        {
            var result = await _service.GetUserAsync(id, UserId);
            //return RedirectToAction("Index",result);
            return Ok(result);
        }

        [HttpGet]
        [Route("User/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var userViewModel = new UserViewModel();
            // userViewModel.Roles =await _roleService.GetRolesAsync();
            return View(userViewModel);
        }

        [HttpPost]
        [Route("User/SaveUserAsync")]
        public async Task<IActionResult> SaveUserAsync(UserViewModel vm)
        {
            var result = await _service.SaveUserAsync(vm, UserId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("User/DeleteUserAsync/{id}")]
        public async Task<IActionResult> DeleteUserAsync(long id)
        {
            var result = await _service.DeleteUserAsync(id, UserId);
            return Ok(result);
        }

        [HttpGet]
        [Route("User/GetRolesAsync")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var result = await _service.GetRolesAsync(UserId);
            return Json(result);
        }
    }
}