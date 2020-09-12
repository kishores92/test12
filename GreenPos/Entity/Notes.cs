using GreenPOS.Entity;
using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Entity
{
    public class Notes : BaseEntity<long>
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }
        
        public long ContactId { get; set; }

    }
}
