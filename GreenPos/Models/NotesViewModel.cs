using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Models
{
    public class NotesViewModel: BaseViewModel<long>
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public long ContactId { get; set; }
    }
}
