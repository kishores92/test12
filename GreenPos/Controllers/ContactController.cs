﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GreenPOS.Common;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GreenPOS.Controllers
{
    public class ContactController : BaseController
    {
        private IContactService _service;
        public ContactController(IContactService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            long User_IDd = (long)UserId;
            var result = await _service.GetContactsAsync(User_IDd);
            ViewBag.contacts = result;
            return View();
        }

        public IActionResult CreateContact()
        {
            
            return View(new ContactViewModel());
        }

        [HttpPost]
        [Route("Contact/SaveContactAsync")]
        public async Task<IActionResult> SaveContactAsync(ContactViewModel vm)
        {
            await _service.SaveContactAsync(vm, UserId);
            // return RedirectToAction("GetContactAsync",new { id = vm.Id });
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Contact/SaveNoteAsync")]
         public async Task<IActionResult> SaveNoteAsync(ContactViewModel vm)
        {
            var title = Request.Form["title"];
            var description = Request.Form["description"];

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(description))
            {
                if (vm.Notes == null)
                {
                    vm.Notes = new List<NotesViewModel>();
                }
                vm.Notes.Add(new NotesViewModel { Title = title, Description = description, ContactId = vm.Id, IsActive = true });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error .. Title or Description cannot be empty  ..");
            }
            await _service.SaveContactAsync(vm, UserId);
            var result = await _service.GetContactAsync(vm.Id, UserId);
            // return RedirectToAction("GetContactAsync",new { id = vm.Id });
            return PartialView("_addNotePartial", result);
        }

        [HttpGet]
        [Route("Contact/DeleteContactAsync/{id}")]
        public async Task<IActionResult> DeleteContactAsync(long id)
        {
            var result = await _service.DeleteContactAsync(id, UserId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Contact/GetContactAsync/{id}")]
        public async Task<IActionResult> GetContactAsync(long id)
        {
            var result = await _service.GetContactAsync(id, UserId);
            return View("CreateContact", result);
        }

    }
}
