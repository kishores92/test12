using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreenPOS.Interfaces;
using GreenPOS.Common;
using GreenPOS.Models;

namespace GreenPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : BaseController
    {
        private INotesService _service;
        public NotesController(INotesService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public async Task<ApiResponse<IEnumerable<NotesViewModel>>> GetAllNotes()
        //{
        //    return await _service.GetAllNotessAsync(UserId);
        //}

        //// GET: api/Notes/5
        //[HttpGet("{id}", Name = "GetNote")]
        //public async Task<ApiResponse<NotesViewModel>> GetNote(int id)
        //{
        //    return await _service.GetNoteByIdAsync(id);
        //}

        //// POST: api/Notes
        //[HttpPost]
        //public async Task<ApiResponse<long>> Save(NotesViewModel vm)
        //{
        //    return await _service.SaveNoteAsync(vm, UserId);
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public async Task<ApiResponse<long>> Delete(int id)
        //{
        //    return await _service.DeleteNoteByIdAsync(id, UserId);
        //}
    }
}
