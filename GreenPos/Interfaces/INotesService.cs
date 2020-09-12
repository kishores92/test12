using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPOS.Common;
using GreenPOS.Models;

namespace GreenPOS.Interfaces
{
    public interface INotesService
    {
        Task<ApiResponse<long>> DeleteNoteByIdAsync(long id, long userId);
        Task<ApiResponse<NotesViewModel>> GetNoteByIdAsync(long id);
        Task<ApiResponse<long>> SaveNoteAsync(NotesViewModel vm, long userId);
        Task<ApiResponse<IEnumerable<NotesViewModel>>> GetAllNotessAsync(long userId);
    }
}
