using Microsoft.AspNetCore.Mvc;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GreenPOS.Controllers
{

    public class RoleController : BaseController
    {
        #region Properties
        public readonly IRoleService _service;
        #endregion

        #region Constructors
        public RoleController(IRoleService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetRolesAsync(UserId);
            ViewBag.roles = result;

            return View();
        }

        [HttpGet]
        [Route("Role/GetRoleByIdAsync/{id}")]
        public async Task<IActionResult> GetRoleByIdAsync(long id)
        {
            var result = await _service.GetRoleByIdAsync(id, UserId);
            return Ok(result);
        }

        [HttpPost]
        [Route("Role/SaveRoleAsync")]
        public async Task<IActionResult> SaveRoleAsync(RoleViewModel vm)
        {
            var result = await _service.SaveRoleAsync(vm, UserId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Role/DeleteRoleAsync/{id}")]
        public async Task<IActionResult> DeleteRoleAsync(long id)
        {
            var result = await _service.DeleteRoleByIdAsync(id, UserId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Role/GetScreenPermissionsAsync")]
        public async Task<IActionResult> GetScreenPermissionsAsync()
        {
            var result = await _service.GetRolePermissionsAsync(UserId);
            return Json(result);
        }
    }
}
